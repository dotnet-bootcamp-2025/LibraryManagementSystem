using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        DbSet<Member> Members { get; set; }
        DbSet<LibraryItem> LibraryItems { get; set; }
        DbSet<BorrowedItem> BorrowedItems { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

    }
}
