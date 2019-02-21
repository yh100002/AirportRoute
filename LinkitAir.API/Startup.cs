using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NJsonSchema;
using NSwag.AspNetCore;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

using LinkitAir.Service.Configurations;
using LinkitAir.Service.Data;
using LinkitAir.Service.ExceptionHandler;
using LinkitAir.Service.Interface;
using LinkitAir.Service.Helpers;
using LinkitAir.Service.Configuration;
using LinkitAir.PerformanceLogger;
using NSwag.SwaggerGeneration.Processors.Security;
using NSwag;
using System.Reflection;

namespace LinkitAir.API
{
    /*
    ASP.NET Core apps use a Startup class, which is named 'Startup' by convention. 
     */
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /*
        Optionally includes a ConfigureServices method to configure the app's services. 
        A service is a reusable component that provides app functionality. 
        Services are configured—also described as registered—in ConfigureServices and consumed across the app via dependency injection (DI) 
        or ApplicationServices. */
        public void ConfigureServices(IServiceCollection services)
        {
            /*Register the context with dependency injection
            all set to connect this database context to your application using the in-memory data provider. 
            */            
            services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase(databaseName: "LinkitAirlineDB"));
            /*
            configuration (e.g. Automapper Profiles) are singletons. 
            That is, they are only ever loaded once when your project runs. 
            This makes sense since your configurations will not change while the application is running.
            The IMapper interface itself is “scoped”. 
            In ASP.net terms, it means that for every individual request, a new IMapper is created but then shared across the entire app for that whole request. 
            So if you use IMapper inside a controller and inside a service for a single request, they will be using the same IMapper instance.           
          
             */
            services.AddAutoMapper();

            /*
            Cross Origin Resource Sharing (CORS) is a W3C standard that allows a server to relax the same-origin policy. 
            Using CORS, a server can explicitly allow some cross-origin requests while rejecting others. 
            */
            services.AddCors();        

            /*
            - Adding the mvc framework which bahaves the latest version 2.2            
            - in a situation where object A has a reference object B and object B has a reference to object A (Circular reference), 
            there is a risk that the serializer will get stuck in a loop endlessly following the references between the objects. 
            To avoid this situation, the serializer throws an exception when it follows a reference to an object that it has already serialized.
            Otherwise Process is terminated due to​​ StackOverflowException!
             */                
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(opt => {
                    opt.SerializerSettings.ReferenceLoopHandling = 
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });              

            services.Configure<DataFileSettings>(Configuration.GetSection("DataFileSettings"));
            services.Configure<ElasticConnectionSettings>(Configuration.GetSection("ElasticConnectionSettings"));

            /*
            * <Service lifetimes>          
            Transient objects are always different; a new instance is provided to every controller and every service.
            Scoped objects are the same within a request, but different across different requests.
            Singleton objects are the same for every object and every request.           
            */
            services.AddSingleton(typeof(ElasticClientProvider));            
            services.AddScoped<IBulkDataLoadRepository, BulkDataLoadRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ISearchMemoryRepository, SearchMemoryRepository>();
            services.AddScoped<ISearchElasticRepository, SearchElasticRepository>();
            services.AddScoped<ILogViewRepository, LogViewRepository>();
            
            /*
            Token authentication is the process of attaching a token (sometimes called an access token or a bearer token) to HTTP requests 
            in order to authenticate them. It’s commonly used with APIs that serve mobile or SPA (JavaScript) clients.
            Each request that arrives at the API is inspected. If a valid token is found, the request is allowed.
            If no token is found, or the token is invalid, the request is rejected with a 401 Unauthorized response.
            Token authentication is usually used in the context of OAuth 2.0 or OpenID Connect. 
             */
            /*
            A symmetric key, also called a shared key or shared secret, is a secret value (like a password) that is kept on both the API (your application) 
            and the authorization server that’s issuing tokens. 
            The authorization server signs the token payload with the shared key, 
            and the API validates that incoming tokens are properly signed using the same key.
            For example only! Don't store your shared keys as strings in code. Use environment variables or the .NET Secret Manager instead.
             */
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            //SeriLog
            var url = Configuration.GetSection("ElasticConnectionSettings:ClusterUrl").Value;
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information() //levels can be overridden per logging source
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(url)) //Logging to Elasticsearch
            {
                AutoRegisterTemplate = true //auto index template like logstash as prefix           
            }).CreateLogger();

        }

        
        /*
        a Configure method to create the app's request processing pipeline.
         */
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IBulkDataLoadRepository seeder, ILoggerFactory loggerFactory)
        { 

            // Handles non-success status codes with empty body
            app.UseExceptionHandler("/errors/500");            
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            //To handle unexpected exception globally, register custome middleware
            app.UseMiddleware<CustomExceptionMiddleware>();        

            //Enable CORS with CORS Middleware for convenience   
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            //Using authentication
            app.UseAuthentication();

            //Enable JWT Authentication into http header
            app.UseSwaggerUi3(typeof(Startup).GetTypeInfo().Assembly, settings =>
            {
                settings.GeneratorSettings.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT Token"));
                settings.GeneratorSettings.DocumentProcessors.Add(new SecurityDefinitionAppender("JWT Token",
                new SwaggerSecurityScheme
                {
                    Type = SwaggerSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    Description = "Copy 'Bearer ' + valid JWT token into field",
                    In = SwaggerSecurityApiKeyLocation.Header
                }));
            });
          
            //Enable PerformanceLogger as middleware layer by using extention
            app.UsePerformanceLog(new LogOptions());
            
            loggerFactory.AddSerilog();
            //Data seeding for convenience
            seeder.LoadData();

            app.UseMvc();
        }
    }
}
