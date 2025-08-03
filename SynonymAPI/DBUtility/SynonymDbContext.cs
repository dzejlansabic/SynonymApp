using Microsoft.EntityFrameworkCore;
using SynonymAPI.Models;

namespace SynonymAPI.DBUtility
{
    public class SynonymDbContext : DbContext
    {
        public SynonymDbContext(DbContextOptions<SynonymDbContext> options) : base(options)
        {
        }
        public DbSet<Word> Words { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Word>()
                .HasMany(w => w.Synonyms)
                .WithOne(w => w.MainSynonym)
                .HasForeignKey(w => w.MainSynonymId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
