using Microsoft.EntityFrameworkCore;
using TechnicalMVC.Models.Entities;

namespace TechnicalMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Client> Clients { get; set; }
    }
}
