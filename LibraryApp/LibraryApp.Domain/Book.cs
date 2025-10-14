namespace LibraryApp.Domain
{
    public class Book : LibraryItem
    {
        public string Author { get; }
        public int Pages { get; }

        public Book(int id, string title, string author) : this(id, title, author, pages: 0)
        {
        }

        public Book(int id, string title, string author, int pages) : base(id, title)
        {
            Author = string.IsNullOrWhiteSpace(author) ? "Unknown" : author.Trim();
            Pages = pages < 0 ? 0 : pages;
        }

        public override string GetInfo()
        {
            return $"Book [Id={Id}, Title={Title}, Author={Author}, Pages={Pages}, IsBorrowed={IsBorrowed}]";
        }
    }
}