using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrartApp.Domain.Entities
{
    public class BorrowItem
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int LibraryItemId { get; set; }

      public Member? Member { get; set; }
        public LibraryItem? LibraryItem { get; set; }

    }
}
