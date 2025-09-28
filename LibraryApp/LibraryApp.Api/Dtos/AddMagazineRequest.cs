namespace LibraryApp.Api.Dtos
{
    public class AddMagazineRequest
    {
        public string? Title { get; set; }
        public int IssueNumber { get; set; }
        public string? Publisher { get; set; }
    }
}
