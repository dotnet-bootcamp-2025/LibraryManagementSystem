using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace LibraryApp.Infraestructure.Data
{
    public class LibraryAppRepository : ILibraryAppRepository
    {
        private readonly AppDbContext _context;

        public LibraryAppRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddLibraryItem(LibraryItem libraryItem)
        {
            _context.LibraryItems.Add(libraryItem);
            _context.SaveChanges();
        }

        public IEnumerable<LibraryItem> GetAllLibraryItems()
        {
            return _context.LibraryItems.ToList();
        }

        public LibraryItem? GetLibraryItemById(int id)
        {
            return _context.LibraryItems.Find(id);
        }

        public void updateLibraryItem(LibraryItem libraryItem)
        {
            _context.LibraryItems.Update(libraryItem);
            _context.SaveChanges();
        }

        public Member? GetMemberById(int id)
        {
            return _context.Members.Find(id);
        }

        public IEnumerable<Member> GetAllMembers()
        {
            return _context.Members.ToList();
        }

        public void AddMember(Member member)
        {
            _context.Members.Add(member);
            _context.SaveChanges();
        }

        public void AddBorrowedItem(BorrowedItem borrowedItem)
        {
            _context.BorrowedItems.Add(borrowedItem);
            _context.SaveChanges();
        }

        public BorrowedItem? GetBorrowedItem(int memberId, int libraryItemId)
        {
            return _context.BorrowedItems
                .FirstOrDefault(b => b.MemberId == memberId && b.LibraryItemId == libraryItemId && b.IsActive);
        }

        public void UpdateLibraryItem(LibraryItem libraryItem)
        {
            _context.LibraryItems.Update(libraryItem);
            _context.SaveChanges();
        }

        // ✅ SOLUCIÓN: Leer la fecha directamente con ADO.NET para evitar conversiones de EF
        public BorrowedItem? GetBorrowedItemByItemId(int libraryItemId)
        {
            var connection = _context.Database.GetDbConnection();
            var wasOpen = connection.State == System.Data.ConnectionState.Open;

            try
            {
                if (!wasOpen) connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT Id, LibraryItemId, MemberId, BorrowDate, IsActive
                    FROM BorrowedItems
                    WHERE LibraryItemId = @libraryItemId AND IsActive = 1
                    ORDER BY BorrowDate DESC
                    LIMIT 1";

                var param = command.CreateParameter();
                param.ParameterName = "@libraryItemId";
                param.Value = libraryItemId;
                command.Parameters.Add(param);

                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var borrowDateString = reader.GetString(3); // BorrowDate column

                    Console.WriteLine($"=== RAW DATE FROM DB ===");
                    Console.WriteLine($"Raw string from SQLite: '{borrowDateString}'");

                    // Parsear la fecha exactamente como está en SQLite
                    DateTime borrowDate;
                    if (DateTime.TryParseExact(borrowDateString,
                        new[] { "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd" },
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out borrowDate))
                    {
                        Console.WriteLine($"Parsed date: {borrowDate:yyyy-MM-dd HH:mm:ss}");
                        Console.WriteLine($"=== END RAW DATE ===");

                        return new BorrowedItem
                        {
                            Id = reader.GetInt32(0),
                            LibraryItemId = reader.GetInt32(1),
                            MemberId = reader.GetInt32(2),
                            BorrowDate = borrowDate,
                            IsActive = reader.GetBoolean(4)
                        };
                    }
                }

                return null;
            }
            finally
            {
                if (!wasOpen) connection.Close();
            }
        }

        public void DeactivateBorrowedItem(int itemId)
        {
            var borrowedItem = _context.BorrowedItems
                .Where(b => b.LibraryItemId == itemId && b.IsActive)
                .OrderByDescending(b => b.BorrowDate)
                .FirstOrDefault();

            if (borrowedItem != null)
            {
                borrowedItem.IsActive = false;
                _context.BorrowedItems.Update(borrowedItem);
                _context.SaveChanges();
            }
        }

        // ✅ MÉTODO PARA TESTING: Insertar préstamo con fecha custom usando SQL directo
        public void InsertBorrowedItemWithCustomDate(int libraryItemId, int memberId, string dateString)
        {
            var connection = _context.Database.GetDbConnection();
            var wasOpen = connection.State == System.Data.ConnectionState.Open;

            try
            {
                if (!wasOpen) connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO BorrowedItems (LibraryItemId, MemberId, BorrowDate, IsActive)
                    VALUES (@libraryItemId, @memberId, @borrowDate, 1)";

                var paramItem = command.CreateParameter();
                paramItem.ParameterName = "@libraryItemId";
                paramItem.Value = libraryItemId;
                command.Parameters.Add(paramItem);

                var paramMember = command.CreateParameter();
                paramMember.ParameterName = "@memberId";
                paramMember.Value = memberId;
                command.Parameters.Add(paramMember);

                var paramDate = command.CreateParameter();
                paramDate.ParameterName = "@borrowDate";
                paramDate.Value = dateString;
                command.Parameters.Add(paramDate);

                command.ExecuteNonQuery();

                Console.WriteLine($"✅ Inserted borrowed item with date: {dateString}");
            }
            finally
            {
                if (!wasOpen) connection.Close();
            }
        }

        public string GetDatabasePath()
        {
            var connection = _context.Database.GetDbConnection();
            var fullPath = System.IO.Path.GetFullPath(connection.DataSource);

            Console.WriteLine($"📂 Database full path: {fullPath}");
            Console.WriteLine($"📂 File exists: {System.IO.File.Exists(fullPath)}");

            return fullPath;
        }

        public List<BorrowedItem> GetBorrowedItemsByMemberId(int memberId)
        {
            return _context.BorrowedItems
                 .AsNoTracking()
                 // Cargamos LibraryItem, pero solo se cargarán los campos.
                 .Include(b => b.LibraryItem)
                 .Where(b => b.MemberId == memberId)
                 .OrderByDescending(b => b.BorrowDate)
                 // 🛑 CRÍTICO: Usamos .Select() para crear nuevas instancias de BorrowedItem 
                 // pero ignoramos explícitamente la propiedad 'BorrowedItems' dentro de LibraryItem.
                 // Esto rompe el ciclo de serialización.
                 .Select(b => new BorrowedItem
                 {
                     Id = b.Id,
                     LibraryItemId = b.LibraryItemId,
                     MemberId = b.MemberId,
                     BorrowDate = b.BorrowDate,
                     IsActive = b.IsActive,
                     // Proyectamos LibraryItem, asegurando que no incluya sus propias relaciones circulares
                     LibraryItem = b.LibraryItem == null ? null : new LibraryItem
                     {
                         Id = b.LibraryItem.Id,
                         Title = b.LibraryItem.Title,
                         IsBorrowed = b.LibraryItem.IsBorrowed,
                         Type = b.LibraryItem.Type,
                         // Añade aquí todas las demás propiedades de LibraryItem que necesites (Author, Pages, etc.)
                         author = b.LibraryItem.author,
                         Pages = b.LibraryItem.Pages,
                         IssueNumber = b.LibraryItem.IssueNumber,
                         Publisher = b.LibraryItem.Publisher,
                         // IMPORTANTE: NO incluimos la propiedad de navegación BorrowedItems aquí
                     },
                     // IMPORTANTE: NO incluimos la propiedad de navegación Member aquí
                 })
                 .ToList();
        }

        public int GetActiveBorrowedItemCountByMemberId(int memberId)
        {
            return _context.BorrowedItems
                .Count(b => b.MemberId == memberId && b.IsActive);
        }

        
    }
}