namespace LibraryApp.Api.Dtos
{
    public class BorrowItemRequest
    {
        public int? MemberId { get; set; }
        public int? ItemId { get; set; }
    }
}
