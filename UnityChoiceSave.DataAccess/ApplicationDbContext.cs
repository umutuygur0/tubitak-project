using Microsoft.EntityFrameworkCore;
using UnityChoiceSave.Models; // Models sınıfın neredeyse o namespace'i ekle

namespace UnityChoiceSave.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<DecisionLog> DecisionLogs { get; set; }
    }
}