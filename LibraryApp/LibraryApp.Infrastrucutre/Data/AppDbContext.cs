using LibrartApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Infrastrucutre.Data
{
    public class AppDbContext : DbContext
    {
        DbSet<Member> Members { get; set; }
        DbSet<BorrowItem> BorrowItems { get; set; }
        DbSet<LibraryItem> LibraryItems { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<Member>(entity =>
        //    {
        //        entity.HasKey(e => e.Id);
        //        entity.Property(e => e.Name).IsRequired();
        //        entity.HasMany(e => e.BorrowedItems)
        //              .WithOne(b => b.Member)
        //              .HasForeignKey(b => b.MemberId);
        //    });

        //    modelBuilder.Entity<LibraryItem>(entity =>
        //    {
        //        entity.HasKey(e => e.Id);
        //        entity.Property(e => e.Title).IsRequired();
        //        entity.Property(e => e.IsBorrowed).HasDefaultValue(false);
        //        entity.HasMany(e => e.BorrowItems)
        //              .WithOne(b => b.LibraryItem)
        //              .HasForeignKey(b => b.LibraryItemId);
        //    });


        //    modelBuilder.Entity<BorrowItem>(entity =>
        //    {
        //        entity.HasKey(e => e.Id);
        //        entity.HasOne(e => e.Member)
        //              .WithMany(m => m.BorrowedItems)
        //              .HasForeignKey(e => e.MemberId);
        //        entity.HasOne(e => e.LibraryItem)
        //              .WithMany(l => l.BorrowItems)
        //              .HasForeignKey(e => e.LibraryItemId);
        //    });


        //}
    }
}