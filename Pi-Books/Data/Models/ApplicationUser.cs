using Microsoft.AspNetCore.Identity;

namespace Pi_Books.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Custom { get; set; }
    }
}