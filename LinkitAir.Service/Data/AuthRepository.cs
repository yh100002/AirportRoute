using System;
using System.Threading.Tasks;
using LinkitAir.Service.ExceptionHandler;
using LinkitAir.Service.Interface;
using LinkitAir.Service.Model;
using Microsoft.EntityFrameworkCore;

namespace LinkitAir.Service.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext context;
        public AuthRepository(DataContext context)
        {
            this.context = context;
        }

        //The name of Method should be LoginAsync
        public async Task<Passenger> Login(string passengerName, string password)
        {   
            var passenger = await context.Passengers.FirstOrDefaultAsync(x => x.PassengerName == passengerName);

            if (passenger == null)
                return null;
            
            if (!VerifyPasswordHash(password, passenger.PasswordHash, passenger.PasswordSalt))
                return null;

            return passenger;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
                return true;
            }
        }

        public async Task<Passenger> Register(Passenger passenger, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            passenger.PasswordHash = passwordHash;
            passenger.PasswordSalt = passwordSalt;

            await this.context.Passengers.AddAsync(passenger);

            await this.context.SaveChangesAsync();

            return passenger;
        }

        /*
        Password Hashing        
         */
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;//Randomly generated seed
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            
        }

        public async Task<bool> PassengerExists(string passengerName)
        {
            if (await this.context.Passengers.AnyAsync(x => x.PassengerName == passengerName))
                return true;

            return false;
        }      
    }
}