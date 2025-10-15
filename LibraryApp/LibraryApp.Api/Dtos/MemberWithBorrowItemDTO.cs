namespace LibraryApp.Api.Dtos
{
    public class MemberWithBorrowItemDTO
    {
        public int MemberId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<string> BorrowedTitles { get; set; } = new();
    }
}
