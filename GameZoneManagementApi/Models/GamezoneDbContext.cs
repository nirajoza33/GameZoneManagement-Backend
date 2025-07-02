//using Microsoft.EntityFrameworkCore;

//namespace GameZoneManagementApi.Models
//{
//    public class GamezoneDbContext :DbContext
//    {
//        public GamezoneDbContext(DbContextOptions<GamezoneDbContext> op):base(op) { }
//        public DbSet<Tblroles> Tblroles { get; set; }
//        public DbSet<Tblusers> Tblusers { get; set; }

//        // In GamezoneDbContext.cs
//        public DbSet<TblGame> Games { get; set; }
//        public DbSet<TblGameCategory> GameCategory { get; set; }

//        public DbSet<TblBooking> TblBookings { get; set; }

//        public DbSet<TblPayment> TblPayments { get; set; }

//        public DbSet<TblReview> TblReviews { get; set; }

//        public DbSet<TblWishlist> TblWishlists { get; set; }



//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<Tblusers>()
//                .HasOne(u => u.Tblrole)
//                .WithMany(r => r.Tblusers)
//                .HasForeignKey(u => u.RoleId);

//            modelBuilder.Entity<TblBooking>()
//                .HasOne(b => b.User)
//                .WithMany()
//                .HasForeignKey(b => b.UserId)
//                .OnDelete(DeleteBehavior.Restrict); // 🚫 prevent cascade delete

//            modelBuilder.Entity<TblBooking>()
//                .HasOne(b => b.Game)
//                .WithMany()
//                .HasForeignKey(b => b.GameId)
//                .OnDelete(DeleteBehavior.Cascade); // this one is allowed

//            modelBuilder.Entity<TblPayment>()
//                .HasOne(p => p.TblUsers)
//                .WithMany()
//                .HasForeignKey(p => p.UserId)
//                .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.NoAction

//            modelBuilder.Entity<TblPayment>()
//                .HasOne(p => p.TblGame)
//                .WithMany()
//                .HasForeignKey(p => p.GameId)
//                .OnDelete(DeleteBehavior.Cascade); // or Restrict/NoAction as needed

//            modelBuilder.Entity<TblReview>()
//                .HasOne(r => r.Game)
//                .WithMany()
//                .HasForeignKey(r => r.GameId)
//                .OnDelete(DeleteBehavior.Cascade); // keep cascade on Game

//            modelBuilder.Entity<TblReview>()
//                .HasOne(r => r.User)
//                .WithMany()
//                .HasForeignKey(r => r.UserId)
//                .OnDelete(DeleteBehavior.Restrict); // prevent cascade on User

//            modelBuilder.Entity<TblWishlist>()
//               .HasOne(w => w.User)
//               .WithMany()
//               .HasForeignKey(w => w.UserId)
//               .OnDelete(DeleteBehavior.Restrict); // Prevent cascade on User deletion

//            modelBuilder.Entity<TblWishlist>()
//                .HasOne(w => w.Game)
//                .WithMany()
//                .HasForeignKey(w => w.GameId)
//                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade on Game deletion



//        }


//    }
//}


using Microsoft.EntityFrameworkCore;
using GameZoneManagementApi.Models;

namespace GameZoneManagementApi.Models
{
    public class GamezoneDbContext : DbContext
    {
        public GamezoneDbContext(DbContextOptions<GamezoneDbContext> op) : base(op) { }

        // Existing DbSets
        public DbSet<Tblroles> Tblroles { get; set; }
        public DbSet<Tblusers> Tblusers { get; set; }
        public DbSet<TblGame> Games { get; set; }
        public DbSet<TblGameCategory> GameCategory { get; set; }
        public DbSet<TblBooking> TblBookings { get; set; }
        public DbSet<TblPayment> TblPayments { get; set; }
        public DbSet<TblReview> TblReviews { get; set; }
        public DbSet<TblWishlist> TblWishlists { get; set; }

        // New DbSets for likes and replies functionality
        public DbSet<TblReviewReply> TblReviewReplies { get; set; }
        public DbSet<TblReviewLike> TblReviewLikes { get; set; }
        public DbSet<TblOffer>  TblOffers { get; set; }
        public DbSet<TblClaimedOffer>  TblClaimedOffers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Existing relationships
            modelBuilder.Entity<Tblusers>()
                .HasOne(u => u.Tblrole)
                .WithMany(r => r.Tblusers)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<TblBooking>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict); // 🚫 prevent cascade delete

            modelBuilder.Entity<TblBooking>()
                .HasOne(b => b.Game)
                .WithMany()
                .HasForeignKey(b => b.GameId)
                .OnDelete(DeleteBehavior.Cascade); // this one is allowed

            modelBuilder.Entity<TblPayment>()
                .HasOne(p => p.TblUsers)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.NoAction

            modelBuilder.Entity<TblPayment>()
                .HasOne(p => p.TblGame)
                .WithMany()
                .HasForeignKey(p => p.GameId)
                .OnDelete(DeleteBehavior.Cascade); // or Restrict/NoAction as needed

            modelBuilder.Entity<TblReview>()
                .HasOne(r => r.Game)
                .WithMany()
                .HasForeignKey(r => r.GameId)
                .OnDelete(DeleteBehavior.Cascade); // keep cascade on Game

            modelBuilder.Entity<TblReview>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict); // prevent cascade on User

            modelBuilder.Entity<TblWishlist>()
               .HasOne(w => w.User)
               .WithMany()
               .HasForeignKey(w => w.UserId)
               .OnDelete(DeleteBehavior.Restrict); // Prevent cascade on User deletion

            modelBuilder.Entity<TblWishlist>()
                .HasOne(w => w.Game)
                .WithMany()
                .HasForeignKey(w => w.GameId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade on Game deletion

            // NEW: Configure TblReviewReply relationships
            modelBuilder.Entity<TblReviewReply>()
                .HasOne(rr => rr.Review)
                .WithMany(r => r.ReviewReplies)
                .HasForeignKey(rr => rr.ReviewId)
                .OnDelete(DeleteBehavior.Cascade); // When review is deleted, delete replies

            modelBuilder.Entity<TblReviewReply>()
                .HasOne(rr => rr.User)
                .WithMany()
                .HasForeignKey(rr => rr.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Don't delete replies when user is deleted

            // NEW: Configure TblReviewLike relationships
            modelBuilder.Entity<TblReviewLike>()
                .HasOne(rl => rl.Review)
                .WithMany(r => r.ReviewLikes)
                .HasForeignKey(rl => rl.ReviewId)
                .OnDelete(DeleteBehavior.Cascade); // When review is deleted, delete likes

            modelBuilder.Entity<TblReviewLike>()
                .HasOne(rl => rl.User)
                .WithMany()
                .HasForeignKey(rl => rl.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Don't delete likes when user is deleted

            // NEW: Ensure unique constraint for user likes (one like per user per review)
            modelBuilder.Entity<TblReviewLike>()
                .HasIndex(rl => new { rl.ReviewId, rl.UserId })
                .IsUnique()
                .HasDatabaseName("IX_TblReviewLikes_ReviewId_UserId");

            // NEW: Configure default values for timestamps
            modelBuilder.Entity<TblReview>()
                .Property(r => r.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<TblReviewReply>()
                .Property(rr => rr.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<TblReviewLike>()
                .Property(rl => rl.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            // NEW: Configure indexes for better performance
            modelBuilder.Entity<TblReviewReply>()
                .HasIndex(rr => rr.ReviewId)
                .HasDatabaseName("IX_TblReviewReplies_ReviewId");

            modelBuilder.Entity<TblReviewReply>()
                .HasIndex(rr => rr.UserId)
                .HasDatabaseName("IX_TblReviewReplies_UserId");

            modelBuilder.Entity<TblReviewReply>()
                .HasIndex(rr => rr.CreatedDate)
                .HasDatabaseName("IX_TblReviewReplies_CreatedDate");

            modelBuilder.Entity<TblReviewLike>()
                .HasIndex(rl => rl.ReviewId)
                .HasDatabaseName("IX_TblReviewLikes_ReviewId");

            modelBuilder.Entity<TblReviewLike>()
                .HasIndex(rl => rl.UserId)
                .HasDatabaseName("IX_TblReviewLikes_UserId");

            modelBuilder.Entity<TblReviewLike>()
                .HasIndex(rl => rl.CreatedDate)
                .HasDatabaseName("IX_TblReviewLikes_CreatedDate");

            // NEW: Configure additional constraints and properties
            modelBuilder.Entity<TblReviewReply>()
                .Property(rr => rr.ReplyText)
                .HasMaxLength(1000)
                .IsRequired();

            modelBuilder.Entity<TblReviewReply>()
                .Property(rr => rr.IsDeleted)
                .HasDefaultValue(false);

            // NEW: Configure TblReview additional properties if they don't exist
            modelBuilder.Entity<TblReview>()
                .Property(r => r.Likes)
                .HasDefaultValue(0);

            modelBuilder.Entity<TblReview>()
                .Property(r => r.Replies)
                .HasDefaultValue(0);

            modelBuilder.Entity<TblReview>()
                .Property(r => r.Sentiment)
                .HasMaxLength(20);

            modelBuilder.Entity<TblReview>()
                .Property(r => r.IsDeleted)
                .HasDefaultValue(false);

            // NEW: Configure soft delete filter (optional - uncomment if you want global query filters)
            /*
            modelBuilder.Entity<TblReview>()
                .HasQueryFilter(r => !r.IsDeleted);

            modelBuilder.Entity<TblReviewReply>()
                .HasQueryFilter(rr => !rr.IsDeleted);
            */


            //--------
            // Configure TblOffer relationships
            modelBuilder.Entity<TblOffer>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure default values
            modelBuilder.Entity<TblOffer>()
                .Property(o => o.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<TblOffer>()
                .Property(o => o.Status)
                .HasDefaultValue(true);

            modelBuilder.Entity<TblOffer>()
                .Property(o => o.IsFeatured)
                .HasDefaultValue(false);

            modelBuilder.Entity<TblOffer>()
                .Property(o => o.IsTrending)
                .HasDefaultValue(false);

            // Configure indexes for better performance
            modelBuilder.Entity<TblOffer>()
                .HasIndex(o => o.UserId)
                .HasDatabaseName("IX_TblOffers_UserId");

            modelBuilder.Entity<TblOffer>()
                .HasIndex(o => o.Category)
                .HasDatabaseName("IX_TblOffers_Category");

            modelBuilder.Entity<TblOffer>()
                .HasIndex(o => o.ValidUntil)
                .HasDatabaseName("IX_TblOffers_ValidUntil");

            modelBuilder.Entity<TblOffer>()
                .HasIndex(o => o.Status)
                .HasDatabaseName("IX_TblOffers_Status");
        }
        public DbSet<GameZoneManagementApi.Models.TblClaimedOffer> TblClaimedOffer { get; set; } = default!;
    }
}