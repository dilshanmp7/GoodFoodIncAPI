using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GoodFoodIncAPI.Models
{
    public partial class GoodFoodIncDBContext : DbContext
    {
        public GoodFoodIncDBContext()
        {
        }

        public GoodFoodIncDBContext(DbContextOptions<GoodFoodIncDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Catagory> Catagories { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<IngredientInfo> IngredientInfoes { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=GoodFoodIncDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Catagory>(entity =>
            {
                entity.ToTable("Catagory");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.ToTable("Ingredient");

                entity.Property(e => e.Slug)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(90);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Ingredients)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ingredient_User");
            });

            modelBuilder.Entity<IngredientInfo>(entity =>
            {
                entity.ToTable("IngredientInfo");

                entity.Property(e => e.Qty)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.Ingredient)
                    .WithMany(p => p.IngredientInfoes)
                    .HasForeignKey(d => d.IngredientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IngredientInfo_Ingredient");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.IngredientInfoes)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IngredientInfo_Recipe");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.ToTable("Recipe");

                entity.Property(e => e.Slug)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(90);

                entity.HasOne(d => d.Catagory)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.CatagoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Recipe_Catagory");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Recipe_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
