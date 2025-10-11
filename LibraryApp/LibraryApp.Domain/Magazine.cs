namespace LibraryApp.Domain;

public sealed class Magazine : LibraryItem //sealed es que no puedo heredar una clase a partir de magazine
{
    public int IssueNumber { get; }
    public string Publisher { get; }

    public Magazine(int id, string title, int issueNumber, string publisher, bool isBorrowed, bool active) : base(id, title, isBorrowed, active)
    {
        IssueNumber = issueNumber;
        Publisher = publisher;
        IsBorrowed =  isBorrowed;
        Active = active;
    }

    
    public override string GetInfo()
        => $"[Magazine] {Title} - Issue #{IssueNumber} ({Publisher}, isBorrowed={IsBorrowed}, Active={Active})";
    
}