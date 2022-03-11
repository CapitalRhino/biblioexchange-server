using AppBackEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppBackEnd.Data
{
    public class AppDbContext : IdentityDbContext<BiblioUser,IdentityRole,string>
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}