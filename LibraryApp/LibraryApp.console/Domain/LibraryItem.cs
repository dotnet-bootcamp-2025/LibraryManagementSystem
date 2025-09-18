namespace LibraryApp.console.Domain
{
    //abstract nunca se instancia
    public abstract class LibraryItem
    {
        public int Id { get; }
        public string Title { get; }
        public bool IsBorrowed { get; private set; }

        // Parameterized constructor (required fields)
        protected LibraryItem(int id, string title)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Id must be positive.");
            Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentException("Title is required.") : title.Trim();
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
        // Polymorphic behavior — each item describes itself
        public abstract string GetInfo();
    }
}
