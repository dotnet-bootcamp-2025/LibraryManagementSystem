namespace LibraryApp.Api.DTOs
{
    //public class BookDto
    //{
    //    public string Title { get; set; }
    //    public string Author { get; set; }
    //    public int Pages { get; set; }
    //}

    public record CreateBookRequest(string Title, string Author, int Pages);
}
