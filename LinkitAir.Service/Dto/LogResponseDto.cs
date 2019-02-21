using System;
using System.Collections.Generic;
using Nest;

namespace LinkitAir.Service.Dto
{   
     /*
    Total number of requests processed
    Total number of requests resulted in an OK, 4xx and 5xx responses
    Average response time of all requests
    Min response time of all requests
    Max response time of all requests */

    public class LogAggregatedValueDto
    {
        public double? TotalCountOfAllRequest { get; set; }
        public double? TotalCountOfNormal { get; set; }
        public double? TotalCountError { get; set; }           
        public double? AverageDurationForAllRequest { get; set; }
        public double? MinDurationForAllRequest { get; set; }
        public double? MaxDurationForAllRequest { get; set; }
        public Dictionary<string, int> CountOfEachErrors { get; set; } = new Dictionary<string, int>();
    }   

/*
For retrieve log data from elasticsearch, you can specify exact name of field with attribute.
 */
    public class LogResponseDto
    {
        public string Message { get; set; }

        [Text(Name = "level")]
        public string Level { get; set; }

        [Date(Name = "@timestamp")]
        public DateTime Timestamp { get; set; }
        public LogResponseFields Fields { get; set; }        
    }

    public class LogResponseFields
    {            
        public string Operation { get; set; }
        
        [Number(Name = "Duration")]
        public double Duration { get; set; }
        [Text(Name = "SourceContext.keyword")]
        public string SourceContext { get; set; }
    }
}