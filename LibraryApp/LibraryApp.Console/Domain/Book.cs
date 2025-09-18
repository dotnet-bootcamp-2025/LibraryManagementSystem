using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Console.Domain
{
    public class Book : LibraryItem
    {
        public string Author { get; }
        public int Pages { get; }

        // Constructor chaining example:
        // Key Benefits
        // 1.	DRY Principle: The validation logic (string.IsNullOrWhiteSpace, pages < 0) exists in only one place
        // 2.	Maintenance: Change validation rules once, affects all constructors
        // 3.	Consistency: All constructors follow the same validation path
        public Book(int id, string title, string author) : this(id, title, author, pages: 0) { }

        public Book(int id, string title, string author, int pages) : base(id, title)
        {
            Author = string.IsNullOrWhiteSpace(author) ? "Unknown" : author.Trim();
            Pages = pages < 0 ? 0 : pages;
        }

        public override string GetInfo() =>
            $"Book [Id={Id}, Title={Title}, Author={Author}, Pages={Pages}, IsBorrowed={IsBorrowed}]";
    }
}
