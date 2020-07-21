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

        public virtual DbSet<Catagory> Catagory { get; set; }
        public virtual DbSet<Ingredient> Ingredient { get; set; }
        public virtual DbSet<IngredientInfo> IngredientInfo { get; set; }
        public virtual DbSet<Recipe> Recipe { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=ENV-ENV767\\SQLSERVER2019;Initial Catalog=GoodFoodIncDB;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Catagory>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.Property(e => e.Slug)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(90);
            });

            modelBuilder.Entity<IngredientInfo>(entity =>
            {
                entity.Property(e => e.Qty)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.Ingredient)
                    .WithMany(p => p.IngredientInfo)
                    .HasForeignKey(d => d.IngredientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IngredientInfo_Ingredient");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.IngredientInfo)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IngredientInfo_Recipe");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.Property(e => e.Slug)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(90);

                entity.HasOne(d => d.Catagory)
                    .WithMany(p => p.Recipe)
                    .HasForeignKey(d => d.CatagoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Recipe_Catagory");

                entity.HasOne(d => d.IngredientInfoNavigation)
                    .WithMany(p => p.RecipeNavigation)
                    .HasForeignKey(d => d.IngredientInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Recipe_IngredientInfo");
            });

            modelBuilder.Entity<User>(entity =>
            {
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
