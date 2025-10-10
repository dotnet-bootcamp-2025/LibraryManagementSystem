namespace LibraryApp.Api.DTOs
{
    public class AddBookDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int Pages { get; set; }
    }
}
