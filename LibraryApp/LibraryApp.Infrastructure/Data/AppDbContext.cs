using LibraryApp.Domain.Entities;
using LibraryApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<LibraryItem> LibraryItems { get; set; }
        public DbSet<BorrowedItem> BorrowedItems { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Member>().HasData(
                new Member { Id = 1, Name = "Alice Johnson", StartDate = new DateTime(2025, 10, 10, 1, 26, 26, 900, DateTimeKind.Local).AddTicks(9253), EndDate = new DateTime(2026, 1, 10, 1, 26, 26, 902, DateTimeKind.Local).AddTicks(7329) },
                new Member { Id = 2, Name = "Bob Smith", StartDate = new DateTime(2025, 10, 10, 1, 26, 26, 900, DateTimeKind.Local).AddTicks(9253), EndDate = new DateTime(2026, 1, 10, 1, 26, 26, 902, DateTimeKind.Local).AddTicks(7329) },
                new Member { Id = 3, Name = "Charlie Brown", StartDate = new DateTime(2025, 10, 10, 1, 26, 26, 900, DateTimeKind.Local).AddTicks(9253), EndDate = new DateTime(2026, 1, 10, 1, 26, 26, 902, DateTimeKind.Local).AddTicks(7329) }
            );

            modelBuilder.Entity<LibraryItem>().HasData(
                new LibraryItem
                {
                    Id = 1,
                    Title = "The Great Gatsby",
                    IsBorrowed = false,
                    Type = (int)LibraryItemTypeEnum.Book,
                    Author = "F. Scott Fitzgerald",
                    Pages = 100
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
