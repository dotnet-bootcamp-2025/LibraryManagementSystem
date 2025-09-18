using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Console.Domain
{
    public class Book : LibraryItem
    {
        public string Author { get; set; }

        public int Pages { get; set; }  

        // Consturctor chaining example
        // Key benefits
        //1. DRY Principle: The validation logic (string.IsNullOrWhiteSpae, pages < 0 exists
        //2. Maintenance: Change validation rules once, affects all constructors
        //3. Consistency: All constructors follow the same validation path

        public Book(int id, string title, string author) : this(id, title, author, pages: 0) 
        {
        }

        public Book(int id, string title, string author, int pages) : base(id, title)      
        {

            Author = string.IsNullOrWhiteSpace(Author) ? "Unknown" : Author.Trim(); 
            Pages = Pages <= 0 ? 0 : Pages;

        }
        public override string GetInfo()
        {
            return $"Book [Id={Id}, Title={Title}, Author={Author}, Pages={Pages}, IsBorrowed={IsBorrowed}]";
        }
   
    }
}
