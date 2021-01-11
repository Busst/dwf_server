using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
#nullable disable

namespace server.models
{
    public partial class dwfContext : DbContext
    {
        private Resources.ServerConfig serverConfig;
        public dwfContext(Resources.ServerConfig serverConfig)
        {
            this.serverConfig = serverConfig;
        }

        public dwfContext(DbContextOptions<dwfContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<FrontPage> FrontPage { get; set; }
        public virtual DbSet<UserLikesRecipes> UserLikesRecipes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .EnableSensitiveDataLogging() 
                    .UseSqlServer($"Server={serverConfig.Name};Database={serverConfig.Schema};Trusted_Connection={serverConfig.Trusted};Uid={serverConfig.UserId};Pwd={serverConfig.Password}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.Property(e => e.Measurement).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Quantity)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.Ingredients)
                    .HasForeignKey(d => d.RecipeId);
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.UserId);

                entity.Property(e => e.Type)
                    .HasConversion(
                        v => v.ToString(),
                        v => (RecipeType)Enum.Parse(typeof(RecipeType), v)
                    );

                entity.Property(e => e.Likes)
                    .HasDefaultValue(0);
                
                entity.HasMany(r => r.Ingredients)
                    .WithOne(i => i.Recipe)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.DisplayName);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(64);

                entity.Property(e => e.AccessToken).HasMaxLength(64);

                entity.Property(e => e.LastLogin);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsFixedLength(true);

                entity.Property(e => e.Username).HasMaxLength(50);

                entity.HasMany(u => u.Recipes)
                    .WithOne(r => r.User)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.ProfilePicture);
                
                entity.Property(e => e.BackgroundPicture);
            });

            modelBuilder.Entity<FrontPage>(entity => {

                entity.Property(e => e.Id);

                entity.Property(e => e.RecipeId);

                entity.Property(e => e.Notes);

                entity.Property(e => e.Type)
                    .HasConversion(
                        v => v.ToString(),
                        v => (FrontPageType)Enum.Parse(typeof(FrontPageType), v));
                    });

            modelBuilder.Entity<UserLikesRecipes>(entity => {
                
                entity.Property(e => e.Id);

                entity.Property(e => e.UserId);

                entity.Property(e => e.RecipeId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
