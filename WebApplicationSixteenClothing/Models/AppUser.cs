using Microsoft.AspNetCore.Identity;

namespace WebApplicationSixteenClothing.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
