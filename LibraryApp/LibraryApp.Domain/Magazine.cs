namespace LibraryApp.Domain;

public sealed class Magazine : LibraryItem //sealed es que no puedo heredar una clase a partir de magazine
{
    public int IssueNumber { get; }
    public string Publisher { get; }

    public Magazine(int id, string title, int issueNumber, string publisher) : base(id, title)
    {
        IssueNumber = issueNumber;
        Publisher = publisher;
    }

    
    public override string GetInfo()
        => $"[Magazine] {Title} - Issue #{IssueNumber} ({Publisher})";
    
}