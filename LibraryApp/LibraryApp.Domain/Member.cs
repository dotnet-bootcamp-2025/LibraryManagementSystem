using System;

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
        if (item == null) throw new ArgumentNullException(nameof(item));
        if (_borrowed.Contains(item)) throw new InvalidOperationException("Item already borrowed by this member.");
        
        item.Borrow();
        _borrowed.Add(item);
    }

    public void ReturnItem(LibraryItem item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        if (!_borrowed.Contains(item)) throw new InvalidOperationException("Item not borrowed by this member.");
        
        item.Return();
        _borrowed.Remove(item);
    }

    public override string ToString() => $"Member {Id}: {Name}, Borrowed Items: {_borrowed.Count}";
}
