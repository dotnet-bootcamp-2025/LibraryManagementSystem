﻿namespace LibraryApp.Domain.Entities
{
    public class Member
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public List<BorrowedItem>? BorrowedItems { get; set; }
    }
}
