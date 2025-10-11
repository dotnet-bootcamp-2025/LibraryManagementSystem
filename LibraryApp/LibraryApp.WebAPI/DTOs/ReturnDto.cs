namespace LibraryApp.WebAPI.DTOs
{
    public record ReturnDto
    {
        public int MemberId { get; init; }
        public int ItemId { get; init; }
    }
}