namespace ApiLibrary.Dto;

public record BookDto
{
    public string Title { get; set; }

    public string Author { get; set; }

    public int Pages { get; set; }

}