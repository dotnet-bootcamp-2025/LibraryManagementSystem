namespace LibraryApp.Api.Records
{
    public class MagazineRecord
    {
        public record CreateMagazineRequest(string Title, int IssueNumber, string Publisher) { }

    }
}
