namespace LibraryApp.Api.Dtos
{
    public class AddBookRequest
    {
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Pages { get; set; }
}
}
