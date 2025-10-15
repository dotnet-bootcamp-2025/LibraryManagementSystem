using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrartApp.Domain.Entities
{
    public class Member
    {
     public int Id { get; set; }
     public required string Name { get; set; }
     public List<BorrowItem> BorrowedItems { get; set; } = new();
    }
}
