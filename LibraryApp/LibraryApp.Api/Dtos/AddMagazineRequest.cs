namespace LibraryApp.Api.Dtos;

public class AddMagazineRequest
{
    public string Title { get; set; } = string.Empty;
    public int IssueNumber { get; set; }
    public string Publisher { get; set; } = string.Empty;
}