using System;
using Nest;

namespace LinkitAir.Service.Model
{    
    public class Passenger
    {
        public Guid PassengerId { get; set; }
        public string PassengerName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}