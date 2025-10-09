using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrartApp.Domain
{
    public class Book : LibraryItem
    {
        public string Author { get; }
        public int Pages { get; }
        public Book(int id, string title, string author, int pages) : base(id, title)
        {
            Author = string.IsNullOrWhiteSpace(author) ? throw new ArgumentException("Author is required.") : author.Trim();
            if (pages <= 0) throw new ArgumentOutOfRangeException(nameof(pages), "Pages must be positive.");
            Pages = pages;
        }
        public override string GetInfo() => $"Book {Id}: '{Title}' by {Author}, {Pages} pages, {(IsBorrowed ? "Borrowed" : "Available")}";


    }

}
