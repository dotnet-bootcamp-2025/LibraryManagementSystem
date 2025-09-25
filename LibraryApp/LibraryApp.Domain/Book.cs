using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Domain;

public class Book : LibraryItem
{
    public string Author { get; }
    public int Pages { get; }

    public Book(int id, string title, string author, int pages) : base(id, title)
    {
        Author = string.IsNullOrWhiteSpace(author) ? "Unknown" : author.Trim();
        Pages = pages < 0 ? 0 : pages;
    }

    public override string GetInfo() => $"[Book] {Title} by {Author}" + (Pages > 0 ? $" ({Pages} pages)" : "");

}
