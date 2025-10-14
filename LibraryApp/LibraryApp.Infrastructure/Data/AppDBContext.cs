using LibraryApp.Domain.Entities;
using LibraryApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Infrastructure.Data
{
    public class AppDBContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<LibraryItem> LibraryItems { get; set; }
        public DbSet<BorrowedItem> BorrowedItems { get; set; }
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BorrowedItem>()
                .Property(b => b.BorrowDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<BorrowedItem>()
                .Property(b => b.ExpirationDate)
                .HasDefaultValueSql("DATE(CURRENT_TIMESTAMP, '+3 days')");

            modelBuilder.Entity<Member>().HasData(
        new Member
        {
        Id = 1,
        Name = "Alice Johnson",
        StartDate = new DateTime(2025, 1, 1),
        EndDate = new DateTime(2025, 12, 31)
        },
        new Member
        {
        Id = 2,
        Name = "Bob Smith",
        StartDate = new DateTime(2025, 1, 1),
        EndDate = new DateTime(2026, 1, 1)
        },
        new Member
        {
        Id = 3,
        Name = "Charlie Brown",
        StartDate = new DateTime(2024, 10, 1),
        EndDate = new DateTime(2025, 4, 1)
        }
    );
            modelBuilder.Entity<LibraryItem>().HasData(
                new LibraryItem
                {
                    Id = 1,
                    Title = "The Great Gatsby",
                    IsBorrowed = false,
                    Type = (int)LibraryItemTypeEnum.Book,
                    Author = "F. Scott Fitzgerald",
                    Pages = 180
                },
                new LibraryItem
                {
                    Id = 2,
                    Title = "1984",
                    IsBorrowed = false,
                    Type = (int)LibraryItemTypeEnum.Book,
                    Author = "George Orwell",
                    Pages = 328
                },
                new LibraryItem
                {
                    Id = 3,
                    Title = "Time Magazine - July 2023",
                    IsBorrowed = false,
                    IssueNumber = 7,
                    Publisher = "Time USA LLC",
                    Type = (int)LibraryItemTypeEnum.Magazine
                }
                );
        }
    }
}