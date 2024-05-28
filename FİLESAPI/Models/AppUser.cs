using Microsoft.AspNetCore.Identity;

namespace FİLESAPI.Models
{
    public class AppUser :IdentityUser
    {
        public string FullName { get; set; }

        public IList<Fillies> Fillies { get; set; }
    }
}