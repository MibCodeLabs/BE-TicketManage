using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ticketManage.Models;

namespace ticketManage.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<ConversationMember>()
                .HasKey(x => new { x.MemberId, x.ConversationId });

            modelBuilder
                .Entity<ConversationMember>()
                .HasOne(x => x.Member)
                .WithMany()
                .HasForeignKey(x => x.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<ConversationMember>()
                .HasOne(x => x.Conversation)
                .WithMany(x => x.Members)
                .HasForeignKey(x => x.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Ticket>()
                .HasOne(x => x.IssuedByUser)
                .WithMany()
                .HasForeignKey(x => x.IssuedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Ticket>()
                .HasOne(x => x.AssignedToUser)
                .WithMany()
                .HasForeignKey(x => x.AssignedToUserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<Chat>()
                .HasOne(x => x.FromUser)
                .WithMany()
                .HasForeignKey(x => x.FromUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Comment>()
                .HasOne(x => x.ParentComment)
                .WithMany(x => x.Replies)
                .HasForeignKey(x => x.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>().HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<Comment>().HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<Chat>().HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<FileAsset>().HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<StorageObject>().HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<Ticket>().Property(t => t.Status).HasConversion<string>();
            modelBuilder.Entity<FileAsset>().Property(f => f.Purpose).HasConversion<string>();
        }

        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Chat> Chats => Set<Chat>();
        public DbSet<Conversation> Conversations => Set<Conversation>();
        public DbSet<ConversationMember> ConversationMembers => Set<ConversationMember>();
        public DbSet<FileAsset> FileAssets => Set<FileAsset>();
        public DbSet<StorageObject> StorageObjects => Set<StorageObject>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    }
}
