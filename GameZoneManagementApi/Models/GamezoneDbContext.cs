using Microsoft.EntityFrameworkCore;

namespace GameZoneManagementApi.Models
{
    public class GamezoneDbContext :DbContext
    {
        public GamezoneDbContext(DbContextOptions<GamezoneDbContext> op):base(op) { }
        public DbSet<Tblroles> Tblroles { get; set; }
        public DbSet<Tblusers> Tblusers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tblusers>()
                .HasOne(u => u.Tblrole)
                .WithMany(r => r.Tblusers)
                .HasForeignKey(u => u.RoleId);
        }

    }
}
