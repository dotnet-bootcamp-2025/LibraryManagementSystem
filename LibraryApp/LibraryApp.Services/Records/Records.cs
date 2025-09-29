namespace LibraryApp.Services.Records;

public sealed record BookRecord(string Title, string Author, int Pages);

public sealed record MagazineRecord(string Title, int IssueNumber, string Publisher);

public sealed record MemberRecord(string Name);

public sealed record BorrowItemRecord(int MemberId, int ItemId);

public sealed record ReturnItemRecord(int MemberId, int ItemId);
