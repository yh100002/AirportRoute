namespace LinkitAir.Service.Dto
{
    public class RouteSearchDto
    {
        private const int MaxPageSize = 200;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 20;
        public int PageSize
        {
            get { return pageSize;}
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value;}
        }
               
        public int MinPrice { get; set; } = 0;  
        public int MaxPrice { get; set; } = 100000;
        public string OrderBy { get; set; }
        public int SourceAirportId { get; set; }        
        public int DestinationAirportId {get; set; }
        public string FullTextSearch {get; set; }
    }
}