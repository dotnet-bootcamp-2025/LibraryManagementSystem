using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Domain.Entities;
using LibraryApp.Domain.Entities.Enums;

namespace LibraryApp.Infraestructure.Data
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
            base.OnModelCreating(modelBuilder); //Adding functionality of the base class

            modelBuilder.Entity<Member>().HasData(
                new Member { Id = 1, Name = "John Doe" },
                new Member { Id = 2, Name = "Jane Smith" },
                new Member { Id = 3, Name = "Alice Johnson" }
            );

            modelBuilder.Entity<LibraryItem>().HasData(
               new LibraryItem
               {
                   Id = 1,
                   Title = "The Great Gatsby",
                   IsBorrowed = false,
                   BorrowedDate = new DateTime(2025,10,08),
                   Type = (int)LibraryItemTypeEnum.Book,
                   Author = "F. Scott Fitzgerald",
                   Pages = 180
               },
               new LibraryItem
            {
            Id = 2,
            Title = "1984",
            IsBorrowed = false,
            BorrowedDate = new DateTime(2025, 10, 08),
            Type = (int)LibraryItemTypeEnum.Book,
            Author = "George Orwell",
            Pages = 328
        },
        new LibraryItem
        {
            Id = 3,
            Title = "Time Magazine - July 2023",
            IsBorrowed = false,
            BorrowedDate = new DateTime(2025, 10, 08),
            IssueNumber = 7,
            Publisher = "Time USA LLC",
            Type = (int)LibraryItemTypeEnum.Magazine
        }
            );
        }

    }
}
