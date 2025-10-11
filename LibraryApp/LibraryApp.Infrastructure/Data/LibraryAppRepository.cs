using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Infrastructure.Data
{
    public class LibraryAppRepository : ILibraryAppRepository
    {
        private readonly AppDbContext _context;

        public LibraryAppRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddBorrowedItem(BorrowedItem borrowedItem)
        {
            _context.BorrowedItems.Add(borrowedItem);
            _context.SaveChanges();
        }

        public void ReturnBorrowedItem(int borrowedItemId)
        {
            var bi = _context.BorrowedItems.Find(borrowedItemId);
            bi.Active = false;
            _context.BorrowedItems.Update(bi);
            _context.SaveChanges();
        }

        public void AddLibraryItem(LibraryItem libraryItem)
        {
            _context.LibraryItems.Add(libraryItem);
            _context.SaveChanges();
        }

        public void AddMember(Member member)
        {
            _context.Members.Add(member);
            _context.SaveChanges();
        }

        public IEnumerable<LibraryItem> GetAllLibraryItems()
        {
            return _context.LibraryItems.ToList();
        }
        public IEnumerable<LibraryItem> GetAllLibraryItemsByMemberId(int memberId)
        {
            // TODO: Change logic to navigation properties
            var bi = GetBorrowedItemsByMember(memberId);
            List<int> itemIds = new List<int>();
            foreach(var item in bi)
            {
                itemIds.Add(item.LibraryItemId);
            }
            
            return _context.LibraryItems
                .Where(li => itemIds.Contains(li.Id))
                .ToList();
        }
        public IEnumerable<Member> GetAllMembers()
        {
            return _context.Members.ToList();

            // Unfinished attemp for returning BorrowedItems
            var query = from member in _context.Members
                        join borrow in _context.BorrowedItems on member.Id equals borrow.MemberId 
                        /*join libItem in _context.LibraryItems on borrow.LibraryItemId equals libItem.Id*/ into borrowedItems
                        select new
                        {
                            Id = member.Id,
                            Name = member.Name,
                            BorrowedItems = (List<BorrowedItem>)borrowedItems
                        };
            
            //foreach (var member in members)
            //{
            //    Console.WriteLine(member.Name);
            //    foreach(var LibItem in member.BorrowedItems)
            //    {
            //        Console.WriteLine($"{LibItem.Id} : {LibItem.Title}");
            //    }
            //}
            var members = (IEnumerable<Member>)query.ToList();
            return members;
        }
        public IEnumerable<BorrowedItem> GetBorrowedItemsByMember(int id)
        {
            return _context.BorrowedItems
                .Where(bi => bi.MemberId == id && bi.Active)
                .ToList()
                ;
        }

        public LibraryItem? GetLibraryItem(int id)
        {
            return _context.LibraryItems.Find(id);

            // NOTA: Forma de traer varios registros
            //var x = _context.LibraryItems.Where(Li => Li.IsBorrowed && Li.Pages > 100).ToList();

            //return x;
        }

        public Member? GetMemberById(int id)
        {
            return _context.Members.Find(id);
        }

        public BorrowedItem? GetBorrowedItem(int memberId, int libraryItemId)
        {
            var bi = _context.BorrowedItems.Where(bi => bi.MemberId == memberId && bi.LibraryItemId == libraryItemId && bi.Active).FirstOrDefault();
            return bi;
        }

        public void UpdateLibraryItem(LibraryItem libraryItem)
        {
            _context.LibraryItems.Update(libraryItem);
            _context.SaveChanges();
        }
    }
}
