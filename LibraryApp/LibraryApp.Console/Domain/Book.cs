using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Console.Domain
{
    public class Book : LibraryItem
    {
        public string Author { get; }

        public int Pages { get; }
        // Parameterized constructor (required fields + specific fields)
        // Call to base class constructor
        public Book(int id, string title, string author, int pages) : base(id, title)
        {
            Author = string.IsNullOrWhiteSpace(author) ? "Unknown Author" : author.Trim();
            Pages = pages < 0 ? 0 : pages;

        }    
        public override string GetInfo()
        {
            return $"Book [Id={Id}, Title={Title}, Author={Author}, Pages={Pages}, IsBorrowed={IsBorrowed}]";
        }

    }
}
