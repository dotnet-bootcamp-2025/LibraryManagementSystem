using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Domain.Entities
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
       
        public ICollection<BorrowedItem> BorrowedItems { get; set; } = new List<BorrowedItem>();

        // Membership properties
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddYears(1);

        // Optional helper
        public bool IsMembershipActive => DateTime.Now <= EndDate;


    }
}
