using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Domain
{
    public sealed class Magazine : LibraryItem
    {
        public int IssueNumber { get; }
        public string Publisher { get; }
        public Magazine(int id, string title, int issueNumber, string publisher, bool isBorrowed)
            : base(id, title, isBorrowed)
        {
            IssueNumber = issueNumber < 0 ? 0 : issueNumber;
            Publisher = string.IsNullOrWhiteSpace(publisher) ? "Unknown" : publisher.Trim();
        }
        public override string GetInfo()
            => $"[Magazine] {Title} - Issue #{IssueNumber} ({Publisher})";
    }
}
