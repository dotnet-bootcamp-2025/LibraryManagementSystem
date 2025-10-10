using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Domain
{
    public sealed class Member
    {
        public int Id { get; }
        public string Name { get; }
        private readonly List<LibraryItem> _borrowed = new();

        public IReadOnlyList<LibraryItem> BorrowedItems => _borrowed;

        public Member(int id, string name)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Id must be positive.");
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Name is required.") : name.Trim();
            Id = id;
        }
        public void BorrowItem(LibraryItem item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            item.Borrow();
            _borrowed.Add(item);
        }
        public void ReturnItem(LibraryItem item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            if (!_borrowed.Remove(item)) throw new InvalidOperationException("This member didn't borrow the item.");
            item.Return();
        }
        public override string ToString() => $"{Id} - {Name} (Borrowed: {_borrowed.Count})";
    }
}
