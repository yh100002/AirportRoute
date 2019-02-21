using System.ComponentModel.DataAnnotations;

namespace LinkitAir.Service.Dto
{
    public class PassengerForRegisterDto
    {
        [Required]
        public string PassengerName { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify a password between 4 and 8 characters")]
        public string Password { get; set; }        
    }
}