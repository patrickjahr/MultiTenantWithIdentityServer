using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Models
{
    public class RegistrationResult
    {
        public RegistrationResultType ResultType { get; set; }
        public IdentityResult IdentityResult { get; set; }
    }
}