using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Infrastructure.Data
{
    public class AppDBContext : DbContext
    {
        DbSet<Member> Members { get; set; }
        DbSet<LibraryItem> LibraryItems { get; set; }
        DbSet<BorrowedItem> BorrowedItems { get; set; }
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
    }
}