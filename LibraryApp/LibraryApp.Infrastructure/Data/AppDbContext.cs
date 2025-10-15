using LibraryApp.Domain.Entities;
using LibraryApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;    

namespace LibraryApp.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<LibraryItems> LibraryItems { get; set; }
        public DbSet<BorrowedItem> BorrowedItems { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Member>().HasData(
                new Member { Id = 1, Name = "John Doe" },
                new Member { Id = 2, Name = "Jane Smith" },
                new Member { Id = 3, Name = "Alice Johnson" }
            );

            modelBuilder.Entity<LibraryItems>().HasData(
                new LibraryItems { 
                    Id = 1, Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    Pages = 180,
                    Type = (int)LibraryItemTypeEnum.Book,
                    IsBorrowed = false 
                }, //acomodarlos
                new LibraryItems { Id = 2, Title = "1984", Author = "George Orwell", Pages = 328, Type = (int)LibraryItemTypeEnum.Book, IsBorrowed = false },
                new LibraryItems { Id = 3, Title = "To Kill a Mockingbird", Author = "Harper Lee", Pages = 281, Type = (int)LibraryItemTypeEnum.Book, IsBorrowed = false },
                new LibraryItems { Id = 4, Title = "Time Magazine - July 2023", IssueNumber = 7, Publisher = "Time USA LLC", Type = (int)LibraryItemTypeEnum.Magazine, IsBorrowed = false },
                new LibraryItems { Id = 5, Title = "National Geographic - August 2023", IssueNumber = 8, Publisher = "National Geographic Partners", Type = (int)LibraryItemTypeEnum.Magazine, IsBorrowed = false }
            );

        }
    }
}
