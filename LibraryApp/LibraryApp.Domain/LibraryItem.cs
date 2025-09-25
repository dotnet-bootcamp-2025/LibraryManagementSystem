using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Domain;
public abstract class LibraryItem
{
    public int Id { get; }

    public string Title { get; }

    public bool IsBorrowed { get; private set; }

    protected LibraryItem(int id, string title)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Id must be psitive");
        Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentException("Title is required.") : title.Trim();

        Id = id;
    }

    public void Borrow()
    {
        if (IsBorrowed) throw new InvalidOperationException("Item already borrower");
        IsBorrowed = true;
    }

    public void Return()
    {
        if (!IsBorrowed) throw new InvalidOperationException("Item is not borrowed");
        IsBorrowed = false;
    }

    public abstract string GetInfo();
}
