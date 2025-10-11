using LibraryApp.Domain.Entities;
using LibraryApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Globalization;

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
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Member>().HasData(
                new Member { Id = 1, Name = "Alice Johnson" },
                new Member { Id = 2, Name = "Bob Smith" },
                new Member { Id = 3, Name = "Charlie Brown" }
            );

            modelBuilder.Entity<LibraryItem>().HasData(
                new LibraryItem
                {
                    Id = 1,
                    Title = "The Great Gatsby",
                    IsBorrowed = false,
                    Type = (int)LibraryItemTypeEnum.Book,
                    author = "F. Scott Fitzgerald",
                    Pages = 180
                },
                new LibraryItem
                {
                    Id = 2,
                    Title = "1984",
                    IsBorrowed = false,
                    Type = (int)LibraryItemTypeEnum.Book,
                    author = "George Orwell",
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

            // ✅ SOLUCIÓN CORRECTA: Convertidor que lee fechas exactamente como están en SQLite
            var dateTimeConverter = new ValueConverter<DateTime, string>(
                // Escribir a DB: Formato ISO 8601 sin timezone
                v => v.ToString("yyyy-MM-dd HH:mm:ss"),
                // Leer de DB: Parsear exactamente como está, sin modificar
                v => DateTime.ParseExact(v, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)
            );

            // Aplicar el convertidor solo a BorrowDate
            modelBuilder.Entity<BorrowedItem>()
                .Property(b => b.BorrowDate)
                .HasColumnType("TEXT")
                .HasConversion(dateTimeConverter);
        }
    }
}