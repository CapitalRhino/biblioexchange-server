using Microsoft.AspNetCore.Identity;

namespace AppBackEnd.Models
{
    public class BiblioUser:IdentityUser
    {
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}