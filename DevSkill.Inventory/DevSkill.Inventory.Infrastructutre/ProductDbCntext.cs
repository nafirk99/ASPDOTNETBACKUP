using DevSkill.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructutre
{
    public class ProductDbCntext : DbContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;

        public ProductDbCntext(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString,
                    x => x.MigrationsAssembly(_migrationAssembly));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(new Category 
            { 
                Id = new Guid("89BBDCC9-43F5-4ACE-ACFE-9CC4DB28EAD8"),
                Name = "General" 
            });
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Product> products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
