using System.Text.Json.Serialization;

namespace LibraryApp.Application.DTOs
{
    public class LoanDetailsDto
    {
        public required string ItemTitle { get; set; }
        [JsonIgnore]
        public DateTime BorrowedDate { get; set; }
        [JsonIgnore]
        public DateTime ReturnDate { get; set; }
        public bool IsExpired { get; set; }

        public string BorrowedDateUS => BorrowedDate.ToString("MM/dd/yyyy");
        public string ReturnDateUS => ReturnDate.ToString("MM/dd/yyyy");
    }
}
