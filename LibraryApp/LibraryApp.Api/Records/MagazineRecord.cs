namespace LibraryApp.Api.Records
{
    public class MagazineRecord
    {
        public record CreateMagazineRecord(string Title, int IssueNumber, string Publisher) { }

    }
}
