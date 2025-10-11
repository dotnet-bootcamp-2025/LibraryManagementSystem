using System.Text.Json.Serialization;

namespace LibraryApp.Domain
{
    public sealed class Member
    {
        public int Id { get; }
        public string Name { get; }

        private readonly List<LibraryItem> _borrowed = new();

        [JsonIgnore]
        public IReadOnlyList<LibraryItem> BorrowedItems => _borrowed;

        public Member(int id, string name)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be positive.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            Id = id;
            Name = name.Trim();
        }

        public void AddBorrowedItem(LibraryItem item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            _borrowed.Add(item);
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