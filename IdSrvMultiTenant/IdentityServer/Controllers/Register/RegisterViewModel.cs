using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Controllers.Register
{
    public class RegisterViewModel
    {
        //[Required]
        public string Tenant { get; set; }
        
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}