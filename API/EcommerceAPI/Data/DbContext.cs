using Microsoft.EntityFrameworkCore;

using EcommerceAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EcommerceAPI.Data
{
    //public class EcommerceContext : DbContext
    public class EcommerceContext : IdentityDbContext<ApplicationUser>
    {
        public EcommerceContext(DbContextOptions<EcommerceContext> options) : base(options) { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}