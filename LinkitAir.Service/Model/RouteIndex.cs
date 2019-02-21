using Nest;

namespace LinkitAir.Service.Model
{
    [ElasticsearchType(Name = "routeindex")]
    public class RouteIndex
    {
        public int RouteId { get; set; }     
        public double FlightFare { get; set; }
        public string DepartureDateTime { get; set; }
        public string ArrivalDateTime { get; set; }         
        public string  SourceAirportName { get; set; } 
        public int  SourceAirportId { get; set; } 
        [Text]
        public string  SourceAirportCity { get; set; } 
        [Text]
        public string  SourceAirportCountry { get; set; } 

        /*
        Elasticsearch is shipped with an in-house solution called 'Completion Suggester'. 
        It uses an in-memory data structure called Finite State Transducer(FST). 
        Elasticsearch stores FST on a per segment basis, which means suggestions scale horizontally as more new nodes are added.
        */
        //The autosuggest items should have completion types as its field type annotation.
        [Completion]             
        public string  DestinationAirportName { get; set; }                                                                                                                                                                         
        public int  DestinationAirportId { get; set; }                                  
        [Text]    
        public string  DestinationAirportCity { get; set; } 
        [Text]
        public string  DestinationAirportCountry { get; set; }   
        public string AirlineName { get; set; }  

         public double SourceLon { get; set; }           
        public double SourceLat { get; set; }           
        public double DestinationLon { get; set; }           
        public double DestinationLat { get; set; }         
    }
}