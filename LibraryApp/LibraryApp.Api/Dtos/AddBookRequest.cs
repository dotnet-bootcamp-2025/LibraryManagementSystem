namespace LibraryApp.Api.Dtos
{
    public class AddBookRequest
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int Pages { get; set; }
    }
}
