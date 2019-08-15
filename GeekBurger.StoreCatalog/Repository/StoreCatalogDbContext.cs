using GeekBurger.StoreCatalog.Model;
using Microsoft.EntityFrameworkCore;

namespace GeekBurger.StoreCatalog.Repository
{
    public class StoreCatalogDbContext : DbContext
    {
        public StoreCatalogDbContext(DbContextOptions<StoreCatalogDbContext> options) : base(options)
        {
        }

        //DbSets
        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Model.Production> Productions { get; set; }
        public DbSet<Restrictions> Restrictions { get; set; }
    }
}
