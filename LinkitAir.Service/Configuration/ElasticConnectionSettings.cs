namespace LinkitAir.Service.Configuration
{
    public class ElasticConnectionSettings
    {
        public string ClusterUrl { get; set; }
        public string AirportIndex { get; set; }
        public string AirlineIndex { get; set; }
        public string RouteIndex { get; set; }
        public string PerformanceLogIndex { get; set; }
        public string RouteDetailedIndex { get; set; }
        public string PerformanceLogType { get; set; }
        public int PerformanceLogMaxSize { get; set; }
      
    }
}