using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LinkitAir.Service.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using LinkitAir.Service.ExceptionHandler;
using System;
using LinkitAir.Service.Interface;
using LinkitAir.Service.Model;
using Nest;
using Microsoft.Extensions.Logging;
using LinkitAir.Service.Configuration;

namespace LinkitAir.Service.Data
{
    public class BulkDataLoadRepository : IBulkDataLoadRepository
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IOptions<DataFileSettings> dataFileSettings;
        private readonly IOptions<ElasticConnectionSettings> esSettings;
        private readonly DataContext dataContext;
        private readonly IElasticClient elasticClient;
        private readonly ILogger<BulkDataLoadRepository> logger;

        public BulkDataLoadRepository(IHostingEnvironment hostingEnvironment, 
                                      IOptions<DataFileSettings> dataFileSettings, 
                                      IOptions<ElasticConnectionSettings> esSettings, 
                                      DataContext dataContext, 
                                      ElasticClientProvider provider, ILogger<BulkDataLoadRepository> logger)       
        {
            this.hostingEnvironment = hostingEnvironment;
            this.dataFileSettings = dataFileSettings;
            this.esSettings = esSettings;
            this.dataContext = dataContext;
            this.elasticClient = provider.Client;
            this.logger = logger;
        }

        //This is invoked by Startup once
        public string LoadData()
        {            
            this.logger.LogInformation($"START LoadData() : {DateTime.UtcNow}");
            if(this.dataContext.Airports.Count() == 0 || this.dataContext.Airlines.Count() == 0 || this.dataContext.Routes.Count() == 0)
            {                
                this.LoadAirports();
                this.LoadAirlines();
                this.LoadRoutes();
                this.IndexingToElasticSearch();
                this.logger.LogInformation($"END LoadData() : {DateTime.UtcNow}");
                return $"Loaded";  
            }          
            
            return $"Seed Data Already Loaded";    
        }  

        private void LoadAirports()
        {
            try
            {
                //To retrive actual local path for data files using hostingEnvironment
                string fullPath = Path.Combine(Path.Combine(this.hostingEnvironment.ContentRootPath, this.dataFileSettings.Value.Folder), this.dataFileSettings.Value.AirportFileName);
              
                using (StreamReader file = File.OpenText(fullPath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    List<Airport> airports = (List<Airport>)serializer.Deserialize(file, typeof(List<Airport>));    
                    foreach(var airport in airports)
                    {
                        this.dataContext.Airports.Add(airport);
                        this.dataContext.SaveChanges();
                        if(!string.IsNullOrEmpty(this.esSettings.Value.AirportIndex))
                            this.elasticClient.IndexDocument(airport);
                    }                  
                }
            }
            catch(Exception ex)
            {
                throw new GlobalDataCustomException("Failed to load Airports data!", ex.Message);
            } 
        }

        private void LoadAirlines()
        {
            try
            {
                string fullPath = Path.Combine(Path.Combine(this.hostingEnvironment.ContentRootPath, this.dataFileSettings.Value.Folder), this.dataFileSettings.Value.AirlineFileName);
              
                using (StreamReader file = File.OpenText(fullPath))
                {
                    JsonSerializer serializer = new JsonSerializer();                             
                    List<Airline> airlines = (List<Airline>)serializer.Deserialize(file, typeof(List<Airline>)); 
                    foreach(var airline in airlines)
                    {
                        this.dataContext.Airlines.Add(airline);
                        this.dataContext.SaveChanges();      
                        if(!string.IsNullOrEmpty(this.esSettings.Value.AirlineIndex))                  
                                this.elasticClient.IndexDocument(airline);
                    }                
                }
            }
            catch(Exception ex)
            {
                throw new GlobalDataCustomException("Failed to load Airlines data!", ex.Message);
            }                   
        }

        private void LoadRoutes()
        {
            try
            {
                string fullPath = Path.Combine(Path.Combine(this.hostingEnvironment.ContentRootPath, this.dataFileSettings.Value.Folder), this.dataFileSettings.Value.RouteFileName);
              
                using (StreamReader file = File.OpenText(fullPath))
                {
                    JsonSerializer serializer = new JsonSerializer();                                                 
                    List<Route> routes = (List<Route>)serializer.Deserialize(file, typeof(List<Route>));   
                    foreach(var route in routes)
                    {
                        this.dataContext.Routes.Add(route);
                        this.dataContext.SaveChanges();
                        if(!string.IsNullOrEmpty(this.esSettings.Value.RouteIndex))
                            this.elasticClient.IndexDocument(route);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new GlobalDataCustomException("Failed to load Route data!", ex.Message);
            }                
        } 

        private void IndexingToElasticSearch()
        {
            try
            {                
                this.IndexSetting();

                IEnumerable<RouteIndex> results = (from r in this.dataContext.Routes
                join a1 in this.dataContext.Airports on r.SourceAirportId equals a1.AirportId
                join a2 in this.dataContext.Airports on r.DestinationAirportId equals a2.AirportId
                join al in this.dataContext.Airlines on r.AirlineID equals al.AirlineId
                select new RouteIndex
                {
                    RouteId = r.RouteId,
                    FlightFare = r.FlightFare,
                    DepartureDateTime = r.DepartureDateTime,
                    ArrivalDateTime = r.ArrivalDateTime,
                    SourceAirportName = a1.AirportName,
                    SourceAirportId = a1.AirportId,
                    DestinationAirportName = a2.AirportName,
                    DestinationAirportId = a2.AirportId,
                    AirlineName = al.AirlineName,
                    DestinationAirportCountry = a2.Country,
                    SourceAirportCountry = a1.Country,
                    SourceAirportCity = a1.City,
                    DestinationAirportCity = a2.City,
                    SourceLat = a1.Lat,
                    SourceLon = a1.Lon,
                    DestinationLat = a2.Lat,
                    DestinationLon = a2.Lon
                });
                
                foreach(var index in results)            
                {
                    var res = this.elasticClient.IndexDocument(index);                    
                }
            
            }
            catch(Exception ex)
            {
                throw new GlobalDataCustomException("Failed to index RouteIndex data!", ex.Message);
            }
        } 

        private void IndexSetting()
        {
            string indexName = this.esSettings.Value.RouteDetailedIndex;
            if (this.elasticClient.IndexExists(indexName).Exists)
            {
                this.elasticClient.DeleteIndex(indexName);
            }
                           
            if(!string.IsNullOrEmpty(this.esSettings.Value.RouteDetailedIndex))
            {
                var indexDescriptor = new CreateIndexDescriptor(indexName)
                                .Mappings(mappings => mappings
                                    .Map<RouteIndex>(m => m.AutoMap()));

                var re = this.elasticClient.CreateIndex(indexName, i => indexDescriptor);
            }                       
        }  
    }        
}
