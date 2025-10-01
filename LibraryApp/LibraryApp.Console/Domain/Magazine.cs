using LibraryApp.Console.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Console.Domain
{
    internal class Magazine
    {
    }
}
public sealed class Magazine : LibraryItem
{
    public int IssueNumber { get; }
    public string Publisher { get; }
    public Magazine(int id, string title, int issueNumber, string publisher)
        : base(id, title)
    {
        IssueNumber = issueNumber < 0 ? 0 : issueNumber;
        Publisher = string.IsNullOrWhiteSpace(publisher) ? "Unknown" : publisher.Trim();
    }
    public override string GetInfo()
        => $"[Magazine] {Title} - Issue #{IssueNumber} ({Publisher})";
}
