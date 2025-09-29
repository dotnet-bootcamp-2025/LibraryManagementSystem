// CQRS - Command Query Responsability Segregation
public class BookDto
{
    public required string Title { get; set; }
    public required string Author { get; set; }
    public int Pages { get; set; }
}