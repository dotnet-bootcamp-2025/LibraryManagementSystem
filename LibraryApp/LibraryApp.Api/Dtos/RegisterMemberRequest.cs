namespace LibraryApp.Api.Dtos;

public class RegisterMemberRequest
{
    public string Name { get; set; } = string.Empty;
    public DateTime? Start { get; set; } = DateTime.Now;
    public DateTime? End { get; }
}