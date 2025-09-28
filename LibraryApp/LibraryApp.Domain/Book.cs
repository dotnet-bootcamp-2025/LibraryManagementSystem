using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibraryApp.Domain
{
    public sealed class Book : LibraryItem
    {
        public string Author { get; }
        public int Pages { get; }

        // Constructor chaining example:
        public Book(int id, string title, string author) : this(id, title, author, pages: 0) { }

        [JsonConstructor]
        public Book(int id, string title, string author, int pages) : base(id, title)
        {
            Author = string.IsNullOrWhiteSpace(author) ? "Unknown" : author.Trim();
            Pages = pages < 0 ? 0 : pages;
        }

        public override string GetInfo()
            => $"[Book] {Title} by {Author}" + (Pages > 0 ? $" ({Pages} pages)" : "");
    }
}
