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

        // Constructor chainning example:
        //key Benefits
        // 1. Code Reusability: By chaining constructors, you can reuse code and avoid duplication.
        // 2. Simplified Object Creation: It simplifies the process of creating objects with different levels of detail.

        public Book(int id, string title, string author) : this(id, title, author, pages: 0) { }
        public Book (int id, string title, string author, int pages) : base(id, title)
        {
            Author = string.IsNullOrWhiteSpace(author) ? "Unknown" : author.Trim();
            Pages = Pages < 0 ? 0 : Pages;
        }
        public override string GetInfo()
        {
            return $"Book [Id={Id}, Title={Title}, Author={Author}, Pages={Pages}, IsBorrowed={IsBorrowed}]";
        }
    }


}
