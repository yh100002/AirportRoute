using LinkitAir.Service.Model;

namespace LinkitAir.Service.Dto
{
    public class RouteSearchResultDto
    {      
        public int RouteId { get; set; }     
        public double FlightFare { get; set; }
        public string DepartureDateTime { get; set; }
        public string ArrivalDateTime { get; set; }         
        public string  SourceAirportName { get; set; } 
        public int  SourceAirportId { get; set; }         
        public string  SourceAirportCity { get; set; }         
        public string  SourceAirportCountry { get; set; }        
        public string  DestinationAirportName { get; set; }  
        public int  DestinationAirportId { get; set; }         
        public string  DestinationAirportCity { get; set; }       
        public string  DestinationAirportCountry { get; set; }   
        public string AirlineName { get; set; }  
        public double SourceLon { get; set; }           
        public double SourceLat { get; set; }           
        public double DestinationLon { get; set; }           
        public double DestinationLat { get; set; }                     
    }
}