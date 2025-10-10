namespace LibraryApp.Domain
{
    public sealed class Member
    {
        public int Id { get; }
        public string Name { get; }
        public string SubscriptionStartDate { get; }

        private readonly List<LibraryItem> _borrowed = new();
        private IReadOnlyList<LibraryItem> BorrowedItems => _borrowed;
        /// <summary>
        /// Made BorrowedItems list private to ensure those items are only get through the made endpoint.
        /// </summary>

        public Member(int id, string name, string memberSince)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Id must be positive.");
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Name is required.") : name.Trim();
            Id = id;
            SubscriptionStartDate = memberSince;
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
