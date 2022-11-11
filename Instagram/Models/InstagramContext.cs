using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Instagram.Models
{
    public partial class InstagramContext : DbContext
    {
        public InstagramContext()
        {
        }

        public InstagramContext(DbContextOptions<InstagramContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Follower> Followers { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=wdb4.my-hosting-panel.com;Database=nurlans1_azegram; User Id=nurlans1_azegram;Password=5Qi51kw0@");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.CommentWriteDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.CommentPost)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.CommentPostId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Comments__Commen__33D4B598");

                entity.HasOne(d => d.CommentUser)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.CommentUserId)
                    .HasConstraintName("FK__Comments__Commen__32E0915F");
            });

            modelBuilder.Entity<Follower>(entity =>
            {
                entity.HasOne(d => d.FollowerFromUser)
                    .WithMany(p => p.FollowerFollowerFromUsers)
                    .HasForeignKey(d => d.FollowerFromUserId)
                    .HasConstraintName("FK__Followers__Follo__403A8C7D");

                entity.HasOne(d => d.FollowerToUser)
                    .WithMany(p => p.FollowerFollowerToUsers)
                    .HasForeignKey(d => d.FollowerToUserId)
                    .HasConstraintName("FK__Followers__Follo__412EB0B6");
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.Property(e => e.GenderName).HasMaxLength(15);
            });

            modelBuilder.Entity<Like>(entity =>
            {
                entity.HasOne(d => d.LikePost)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.LikePostId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Likes__LikePostI__3D5E1FD2");

                entity.HasOne(d => d.LikeUser)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.LikeUserId)
                    .HasConstraintName("FK__Likes__LikeUserI__3C69FB99");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.PostDescription).HasMaxLength(500);

                entity.Property(e => e.PostPhotoUrl).HasMaxLength(20);

                entity.Property(e => e.PostWriteDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.PostUser)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.PostUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Posts__PostUserI__29572725");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserBio).HasMaxLength(100);

                entity.Property(e => e.UserName).HasMaxLength(15);

                entity.Property(e => e.UserNickName)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.UserPassword).HasMaxLength(30);

                entity.Property(e => e.UserProfilePhotoUrl).HasMaxLength(20);

                entity.Property(e => e.UserSurname).HasMaxLength(15);

                entity.HasOne(d => d.UserGender)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserGenderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Users__UserGende__267ABA7A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
