using Nest;

namespace LinkitAir.Service.Model
{
    [ElasticsearchType(Name = "airport")]
    public class Airport
    {
        public int AirportId { get; set; }
        [Completion]
        public string AirportName { get; set; }
        [Text]
        public string City { get; set; }
        [Text]
        public string Country { get; set; }
        public string AirportIATA { get; set; }
        public string AirportICAO { get; set; }
        
        public double Lat { get; set; }
        
        public double Lon { get; set; }
    }
}