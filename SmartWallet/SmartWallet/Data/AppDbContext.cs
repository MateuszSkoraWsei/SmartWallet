using Microsoft.EntityFrameworkCore;

namespace SmartWallet.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Models.ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Models.Category> Categories { get; set; }
        public DbSet<Models.Transactions> Transactions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Models.Transactions>()
                                .HasOne(t => t.Sender)
                .WithMany(u => u.SentTransaction)
                .HasForeignKey(t => t.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Models.Transactions>()
                .HasOne(t => t.Receiver)
                .WithMany(u => u.ReceivedTransaction)
                .HasForeignKey(t => t.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
