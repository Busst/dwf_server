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
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("FK_Ingredients_Recipes");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Recipes_Users");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.DisplayName)
                    .HasMaxLength(30)
                    .IsFixedLength(true)
                    .HasDefaultValue("");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(64);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsFixedLength(true);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
