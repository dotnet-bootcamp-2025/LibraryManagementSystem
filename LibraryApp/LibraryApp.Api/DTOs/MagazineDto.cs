namespace LibraryApp.Api.DTOs
{
    //public class MagazineDto
    //{
    //    public string Title { get; set; }
    //    public int IssueNumber { get; set; }
    //    public string Publisher { get; set; }
    //}

    public record CreateMagazineRequest(string Title, int IssueNumber, string Publisher);
}
