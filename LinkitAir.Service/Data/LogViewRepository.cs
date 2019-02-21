using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using LinkitAir.Service.ExceptionHandler;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Elasticsearch.Net;
using System;
using Microsoft.Extensions.Options;
using LinkitAir.Service.Configuration;
using LinkitAir.Service.Dto;
using System.Collections.Generic;
using LinkitAir.Service.Interface;

namespace LinkitAir.Service.Data
{
    public class LogViewRepository : ILogViewRepository
    {
        private readonly IElasticClient elasticClient;     
        private readonly IOptions<ElasticConnectionSettings> elasticSettings;    


        public LogViewRepository(IOptions<ElasticConnectionSettings> elasticSettings, ElasticClientProvider provider)
        {
            this.elasticClient = provider.Client;
            this.elasticSettings = elasticSettings;
        }

        public double? Total()
        {
            if (this.elasticClient.IndexExists(this.elasticSettings.Value.PerformanceLogIndex).Exists)
            {               
                var result = this.elasticClient.Search<LogResponseDto>(searchDescriptor => searchDescriptor
                                                .Index(this.elasticSettings.Value.PerformanceLogIndex)
                                                .Type(this.elasticSettings.Value.PerformanceLogType)
                                                .Size(0)                                              
                                                .Aggregations(aggs => aggs.ValueCount("total_count", c => c.Field(f => f.Fields.SourceContext))));;

                return result.Aggregations.ValueCount("total_count").Value;     
            }     

            throw new Exception($"{this.elasticSettings.Value.PerformanceLogIndex} Index is not found!");                    
        }

        //information level or error level
        public int CountOnLevel(string value)
        {
            if (this.elasticClient.IndexExists(this.elasticSettings.Value.PerformanceLogIndex).Exists)
            {
                var result = this.elasticClient.Search<LogResponseDto>(searchDescriptor => searchDescriptor
                                                .Index(this.elasticSettings.Value.PerformanceLogIndex)
                                                .Type(this.elasticSettings.Value.PerformanceLogType)
                                                .Size(this.elasticSettings.Value.PerformanceLogMaxSize)                                        
                                                .Query(queryContainerDescriptor => queryContainerDescriptor                                       
                                                .Term(t => t.Name("level").Field(f => f.Level).Value(value)))).Documents;     

                return result.Count;      
            }     

             throw new Exception($"{this.elasticSettings.Value.PerformanceLogIndex} Index is not found!");                
        }

        public LogAggregatedValueDto AggregatedValuesForAll()
        {
            if (this.elasticClient.IndexExists(this.elasticSettings.Value.PerformanceLogIndex).Exists)
            {
                var result = this.elasticClient.Search<LogResponseDto>(searchDescriptor => searchDescriptor
                                                .Index(this.elasticSettings.Value.PerformanceLogIndex)
                                                .Type(this.elasticSettings.Value.PerformanceLogType)
                                                .Size(0) //very powerful                                             
                                                .Aggregations(aggs => aggs
                                                            .Average("duration_average", avg => avg.Field(f => f.Fields.Duration))
                                                            .Max("duration_max", max => max.Field(f => f.Fields.Duration))
                                                            .Min("duration_min", min => min.Field(f => f.Fields.Duration))));               

                return new LogAggregatedValueDto
                {
                    AverageDurationForAllRequest = result.Aggregations.Average("duration_average").Value,
                    MaxDurationForAllRequest = result.Aggregations.Max("duration_max").Value,
                    MinDurationForAllRequest = result.Aggregations.Min("duration_min").Value
                };
            }     

            throw new Exception($"{this.elasticSettings.Value.PerformanceLogIndex} Index is not found!");                            
        }

        public Dictionary<string, int> GetErrorSet()
        {
            if (this.elasticClient.IndexExists(this.elasticSettings.Value.PerformanceLogIndex).Exists)
            {
                var result = this.elasticClient.Search<LogResponseDto>(searchDescriptor => searchDescriptor
                                                .Index(this.elasticSettings.Value.PerformanceLogIndex)
                                                .Type(this.elasticSettings.Value.PerformanceLogType)
                                                .Size(this.elasticSettings.Value.PerformanceLogMaxSize)                                        
                                                .Query(queryContainerDescriptor => queryContainerDescriptor                                       
                                                .Term(t => t.Name("level").Field(f => f.Level).Value("error")))).Documents;

                Dictionary<string, int> errorSet = new Dictionary<string, int>();
                
                foreach(var error in result)
                {                    
                    if(errorSet.TryGetValue(error.Message, out int cnt))
                    {                        
                        errorSet[error.Message]++;        
                    }
                    else
                    {
                        errorSet.Add(error.Message, 1);
                    }
                }    

                errorSet.Add("Total", result.Count);           

                return errorSet;      
            }     

             throw new Exception($"{this.elasticSettings.Value.PerformanceLogIndex} Index is not found!");                
        }        
    }
}