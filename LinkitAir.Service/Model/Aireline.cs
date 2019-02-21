using Nest;

namespace LinkitAir.Service.Model
{
    [ElasticsearchType(Name = "airline")]
    public class Airline
    {    
        public int AirlineId { get; set; }
        [Text]
        public string AirlineName { get; set; }
        public string AirlineIATA { get; set; }
        public string AirlineICAO { get; set; }
        [Text]
        public string Country { get; set; }
    
    }
}