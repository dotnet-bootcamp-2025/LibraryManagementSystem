using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Domain
{
    public sealed class BorrowedItem
    {
        public int Id { get; }
        public int MemberId { get; }
        public int LibraryItemId { get; }
        public DateTime BorrowDate { get; }
        public bool Active { get; }

        // Constructor chaining example:

        public BorrowedItem(int id, int memberId, int libraryItemId) : this(id, memberId, libraryItemId, borrowDate: DateTime.UtcNow.AddDays(3) , true)
        {
        }
        public BorrowedItem(int id, int memberId, int libraryItemId, DateTime borrowDate, bool active)
        {
            Id = id;
            MemberId = memberId;
            LibraryItemId = libraryItemId;
            BorrowDate = borrowDate;
            Active = active;
        }
    }
}
