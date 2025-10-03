using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibraryApp.Domain
{
    public class Book : LibraryItem
    {
        public string Author { get; }
        public int Pages { get; }

        //Constructor chaining example:
        //key benefits
        // 1. dont repeat yourself, no repitas el esfuerzo de algo ya validado
        // 2. mantenimiento: change validation rules once, affects all constructors
        // 3. consistency: all constructors follow the same validation path

        public Book(int id, string title, string author) : this(id, title, author, pages: 0) { }

        
        public Book(int id, string title, string author, int pages) : base(id, title)
        {
            Author = string.IsNullOrWhiteSpace(author) ? "Unknown" : author.Trim();
            Pages = Pages < 0 ? 0 : Pages;
        }


        public override string GetInfo() =>
            $"Book [Id={Id}, Title={Title}, Author={Author}, Pages={Pages}, IsBorrowed ={IsBorrowed}]";

    }
}
