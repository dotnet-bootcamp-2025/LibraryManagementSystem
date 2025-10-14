using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LibraryApp.Domain
{
    public class Member
    {
        public int Id { get; }
        public string Name { get; }
        private readonly List<LibraryItem> _borrowed = new();
        public IReadOnlyList<LibraryItem> BorrowedItems => _borrowed;
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        [JsonConstructor] // Decorador que permite dar soporte a la deserialización
        public Member(int id, string name)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Id must be positive.");
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Name is required.") : name.Trim();
            Id = id;
            StartDate = DateTime.Now;
            EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 0).AddMonths(3);
        }
        // Constructor para permitir añadir un valor de StartDate y EndDate
        public Member(int id, string name, DateTime startDate, DateTime endDate)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Id must be positive.");
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Name is required.") : name.Trim();
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
        }
        public void BorrowItem(LibraryItem item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            item.Borrow();
            _borrowed.Add(item);
        }
        public void ReturnItem(LibraryItem item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            if (!_borrowed.Remove(item)) throw new InvalidOperationException("This member didn't borrow the item.");
            item.Return();
        }
        public override string ToString() => $"{Id} - {Name} (Borrowed: {_borrowed.Count})";
    }
}
