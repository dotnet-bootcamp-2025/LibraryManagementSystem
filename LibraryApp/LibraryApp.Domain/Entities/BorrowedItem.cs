using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Domain.Entities
{
    public class BorrowedItem
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int LibraryItemId { get; set; }
        public DateTime BorrowDate { get; set; }
        public bool Active { get; set; }

        public Member? Member { get; set; }
        public List<BorrowedItem>? BorrowedItems { get; set; }
    }
}
