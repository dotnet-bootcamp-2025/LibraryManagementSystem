using System;

/// <summary>
/// Summary description for Class1
/// </summary>
public sealed class Magazine: LibraryItem
{
	public int IssueNumber { get; }
	public string Publisher { get; }
	public Magazine(int id, string title, int issueNumber, string publisher) : base(id, title)
	{
		if (issueNumber <= 0) throw new ArgumentOutOfRangeException(nameof(issueNumber), "Issue number must be positive.");
		IssueNumber = issueNumber;
		Publisher = string.IsNullOrWhiteSpace(publisher) ? throw new ArgumentException("Publisher is required.") : publisher.Trim();
    }
	public override string GetInfo() => $"Magazine {Id}: '{Title}', Issue {IssueNumber}, Published by {Publisher}, {(IsBorrowed ? "Borrowed" : "Available")}";
}
