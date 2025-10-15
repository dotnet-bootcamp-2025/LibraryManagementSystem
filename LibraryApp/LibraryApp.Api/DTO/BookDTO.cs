namespace LibraryApp.Api.DTO
{
    public record BookDTO
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public int Pages { get; set; }
    }
}
