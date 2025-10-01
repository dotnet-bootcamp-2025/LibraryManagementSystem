using System.Reflection;

namespace LibraryApp.Console.Domain
{
    public sealed class Magazine : LibraryItem
    {
        public int IssueNumber { get; }

        public string Publisher { get; }

        public Magazine(int id, string title, int issueNumber, string publisher) : base(id, title)
        {
            IssueNumber = issueNumber < 0 ? 0 : issueNumber;
            Publisher = string.IsNullOrWhiteSpace(publisher) ? "Unknown" : publisher.Trim();
        }

        public override string GetInfo()
        {
            return $"Magazine [Id={Id}, Title={Title}, IssueNumber={IssueNumber}, Publisher={Publisher}, IsBorrowed={IsBorrowed}]";
        }

    }
}
