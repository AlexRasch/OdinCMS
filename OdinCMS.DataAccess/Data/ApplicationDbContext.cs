using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OdinCMS.Models;

namespace OdinCMS.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        /* Products */
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CoverType> CoverTypes { get; set; }
        
        /*  Cart */
        public DbSet<ShoppingCart> ShoppingCarts { get; set; } 
        
        /* Order */
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        /* Customer / Company / User */
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companys { get; set; }
    }
}
