namespace LibraryApp.Api.DTOs
{
    //public class BorrowDto
    //{
    //    public int memberId { get; set; }
    //    public int itemId{ get; set; }
    //    public string message{ get; set; }
    //}

    public record CreateBorrowRequest(int memberId, int itemId, string message);
}
