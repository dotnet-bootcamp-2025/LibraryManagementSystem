
using LibrartApp.Domain.Entities;
using LibrartApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<BorrowItem> BorrowItems { get; set; }
        public DbSet<LibraryItem> LibraryItems { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           base.OnModelCreating(modelBuilder);

           modelBuilder.Entity<Member>().HasData(
               new Member { Id = 1, Name = "Alice Johnson" },
               new Member { Id = 2, Name = "Bob Smith" },
               new Member { Id = 3, Name = "Charlie Brown" }
           );

            modelBuilder.Entity<LibraryItem>().HasData(
               new LibraryItem { Id = 1, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Pages = 180, Type = (int)LibraryItemTypeEnum.Book },
               new LibraryItem { Id = 2, Title = "1984", Author = "George Orwell", Pages = 328, Type = (int)LibraryItemTypeEnum.Book },
               new LibraryItem { Id = 3, Title = "To Kill a Mockingbird", Author = "Harper Lee", Pages = 281, Type = (int)LibraryItemTypeEnum.Book },
               new LibraryItem { Id = 4, Title = "Time Magazine - July 2023", IssueNumber = 7, Publisher = "Time USA, LLC", Type = (int)LibraryItemTypeEnum.Magazine },
               new LibraryItem { Id = 5, Title = "National Geographic - August 2023", IssueNumber = 8, Publisher = "National Geographic Partners", Type = (int)LibraryItemTypeEnum.Magazine }
               );



        }
    }
}


