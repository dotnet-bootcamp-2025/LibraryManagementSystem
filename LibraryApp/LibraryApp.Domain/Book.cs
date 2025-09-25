using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Domain
{
    public class Book : LibraryItem
    {
        public string Author { get; }

        public int Pages { get; }

        //constructor chaining example

        public Book(int id, string title, string author) : this(id, title, author, pages: 0) { }
        public Book(int id, string title, string author, int pages) : base(id, title)
        {
            Author = string.IsNullOrWhiteSpace(author) ? "Unknow" : author.Trim();
            pages = pages < 0 ? 0 : pages;
        }

        public override string GetInfo() =>

             $"Book [Id={Id}, Title={Title}, Author={Author}, Pages={Pages}, IsBorrowed={IsBorrowed}]";

    }
}
