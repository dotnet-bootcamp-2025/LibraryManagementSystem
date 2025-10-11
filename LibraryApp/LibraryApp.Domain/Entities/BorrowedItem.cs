using System;
namespace LibraryApp.Domain.Entities;

public class BorrowedItem
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int LibraryItemId { get; set; }
    public DateTime? BorrowedDate { get; set; }
    public bool Active { get; set; } = true;
    public Member? Member { get; set; }
    public LibraryItem? LibraryItem { get; set; }
}