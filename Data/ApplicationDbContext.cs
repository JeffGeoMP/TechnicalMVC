using Microsoft.EntityFrameworkCore;
using TechnicalMVC.Models.Entities;

namespace TechnicalMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            base.SavingChanges += new EventHandler<SavingChangesEventArgs>(OnSavingEvent);
        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<FamilyProducts> FamilyProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Quotation> Quotations { get; set; }
        public DbSet<QuotationDetail> QuotationDetails { get; set; }

        public void OnSavingEvent(object sender, SaveChangesEventArgs e)
        {
            var applicationDbContext = (ApplicationDbContext)sender;
            var entries = applicationDbContext.ChangeTracker.Entries().ToList();
            entries.ForEach((e) =>
            {
                if (e.Entity is AttributesLog entity)
                {
                    switch (e.State)
                    {
                        case EntityState.Added:
                            entity.CreatedAt = entity.UpdatedAt = DateTime.UtcNow;
                            break;
                        case EntityState.Modified:
                            entity.UpdatedAt = DateTime.UtcNow;
                            break;
                        default:
                            break;
                    }
                }
            });
        }

    }
}
