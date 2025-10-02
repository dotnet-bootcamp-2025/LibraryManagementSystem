using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Domain
{
    public abstract class LibraryItem
    {
        public int Id { get; }
        public string Title { get; set; }
        public bool IsBorrowed { get; private set; }

        // Parameterized constructor (required fields)
        public LibraryItem(int id, string title)
        {
            Title = title;
            Id = id;
        }
        public void Borrow()
        {
            if (IsBorrowed) throw new InvalidOperationException("Item already borrowed.");
            IsBorrowed = true;
        }
        public void Return()
        {
            if (!IsBorrowed) throw new InvalidOperationException("Item is not borrowed.");
            IsBorrowed = false;
        }
        public abstract string GetInfo();  //forsando a las clases hijas a implementar este metodo
    }
}