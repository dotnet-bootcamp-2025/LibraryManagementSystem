namespace LibraryApp.Services.Records;

public sealed record BookRecord(string Title, string Author, int Pages);

public sealed record MagazineRecord(string Title, int IssueNumber, string Publisher);

public sealed record MemberRecord(string Name);
