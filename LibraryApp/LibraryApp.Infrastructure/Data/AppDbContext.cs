using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LibraryApp.Infrastructure.Data
{
    public class AppDbContext: DbContext
    {
        DbSet<Domain.Entities.Member> Members { get; set; }

        DbSet<Domain.Entities.LibraryItem> LibraryItems { get; set; }

        DbSet<Domain.Entities.BorrowedItem> BorrowedItems { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
