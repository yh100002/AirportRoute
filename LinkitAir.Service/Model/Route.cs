using System;
using Nest;

namespace LinkitAir.Service.Model
{
    [ElasticsearchType(Name = "route")]
    public class Route
    {
        public int RouteId { get; set; }
        public string IATA { get; set; }
        public int AirlineID { get; set; }
        public string SourceAirport { get; set; }
        public int SourceAirportId { get; set; }        
        public string DestinationAirport { get; set; }
        public int DestinationAirportId { get; set; }
        public string PlaneName { get; set; }
        public double FlightFare { get; set; }
        public string DepartureDateTime { get; set; }
        public string ArrivalDateTime { get; set; }
    
    }
}