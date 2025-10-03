namespace LibraryApp.Api.Dtos
{
    //esta y las otras clases dto son modelos de solicitud
    public class AddMagazineRequest
    {
        public string? Title { get; set; }
        public int IssueNumber { get; set; }
        public string? Publisher { get; set; }
    }
}
