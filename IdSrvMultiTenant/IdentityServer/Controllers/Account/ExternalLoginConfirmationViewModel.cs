using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Controllers.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "EMAIL_REQUIRED")]
        [EmailAddress(ErrorMessage = "EMAIL_INVALID")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "TENANT_REQUIRED")]
        public string Tenant { get; set; }
    }
}