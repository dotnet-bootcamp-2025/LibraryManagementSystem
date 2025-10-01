using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Infraestructure.Data
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
