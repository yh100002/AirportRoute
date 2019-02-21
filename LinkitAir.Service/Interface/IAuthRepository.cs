using System.Threading.Tasks;
using LinkitAir.Service.Model;

namespace LinkitAir.Service.Interface
{
    public interface IAuthRepository
    {
         Task<Passenger> Register(Passenger passenger, string password);
         Task<Passenger> Login(string passengerName, string password);
         Task<bool> PassengerExists(string passengerName);
    }
}