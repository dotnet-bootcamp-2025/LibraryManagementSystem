namespace LibraryApp.Domain
{
    public sealed class Magazine : LibraryItem
    {
        public int IssueNumber { get; }
        public string Publisher { get; }

        public Magazine(int id, string title, int issueNumber, string publisher, bool isBorrowed = false) : base(id, title, isBorrowed)
        {
            IssueNumber = issueNumber < 0 ? 0 : issueNumber;
            Publisher = string.IsNullOrWhiteSpace(publisher) ? "Unknown" : publisher.Trim();
        }

        public override string GetInfo()
        {
            return $"[Magazine] {Title} - Issue #{IssueNumber} ({Publisher})";
        }
    }
}
