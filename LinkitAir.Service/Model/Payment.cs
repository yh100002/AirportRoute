using System;
using Nest;

namespace LinkitAir.Service.Model
{
    [ElasticsearchType(Name = "payment")]
    public class Payment
    {
        public Guid PaymentId { get; set; }
        public int RouteId { get; set; }
        public int PassengerId { get; set; }

    }
}