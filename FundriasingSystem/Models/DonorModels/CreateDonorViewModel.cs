using System.ComponentModel.DataAnnotations;

namespace FundriasingSystem.Models.DonorModels
{
    public class CreateDonorViewModel
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
