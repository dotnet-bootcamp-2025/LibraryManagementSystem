using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Mappers;
using LibraryApp.Domain;
using LibraryApp.Domain.Entities;
using LibraryApp.Domain.Enums;
using LibraryItem = LibraryApp.Domain.Entities.LibraryItem;
using Member = LibraryApp.Domain.Entities.Member;

namespace LibraryApp.Application.Services
{
    public sealed class LibraryService : ILibraryService
    {
        private readonly ILibraryAppRepository _repository;
        public LibraryService(ILibraryAppRepository repository)
        {
            _repository = repository;
        }

        public Book AddBook(string title, string author, int pages = 0)
        {
            var bookEntity = new LibraryItem
            {
                Title = title,
                Author = author,
                Pages = pages,
                Type = (int)LibraryItemTypeEnum.Book,
                IsBorrowed = false,
                Active = true
            };
            _repository.AddLibraryItem(bookEntity);

            return new Book(bookEntity.Id, bookEntity.Title ?? string.Empty, bookEntity.Author ?? string.Empty, bookEntity.Pages ?? 0, bookEntity.IsBorrowed, bookEntity.Active);
        }

        public Magazine AddMagazine(string title, int issueNumber, string publisher)
        {
            var magEntity = new LibraryItem
            {
                Title = title,
                IssueNumber = issueNumber,
                Publisher = publisher,
                Type = (int)LibraryItemTypeEnum.Magazine,
                IsBorrowed = false,
                Active = true
            };
            _repository.AddLibraryItem(magEntity);
            return new Magazine(magEntity.Id, magEntity.Title ?? string.Empty, magEntity.IssueNumber ?? 0, magEntity.Publisher ?? string.Empty, magEntity.IsBorrowed, magEntity.Active);
        }
        
        public IEnumerable<Domain.Member> GetAllMembers()
        {
            var entities = _repository.GetAllMembers() ?? Enumerable.Empty<Domain.Entities.Member>();
            return entities.Select(e => new Domain.Member(e.Id, e.Name ?? string.Empty));
        }

        public IEnumerable<BorrowedItem> GetAllBorrowedItems()
        {
            var entities = _repository.GetAllBorrowedItems() ?? Enumerable.Empty<BorrowedItem>();
            return entities;
        }

        public bool MembershipStatus(int memberId,  out string message)
        {
            var member = _repository.GetMemberById(memberId);
            if (member is null)
            {
                message = "Member not found.";
                return false;
            }

            if (!member.EndDate.HasValue)
            {
                message = "Your membership is active";
                return true;
            }
            var currentDate = DateTime.Now;
            if (currentDate >= member.EndDate)
            {
                message = "Your membership is over";
                System.Console.WriteLine("Your membership is over");
                return false;
            }
            message = "Your membership is active";
            return true;
        }

        public Domain.Member RegisterMember(string name, DateTime? StartDate, DateTime? EndDate)
        {
            var membershipStart = StartDate?.Date ?? DateTime.Today;
            var membershipEnd = EndDate?.Date ?? membershipStart.AddYears(1);
            var memberEntity = new Member { Name = name , StartDate = membershipStart, EndDate = membershipEnd };
            _repository.AddMember(memberEntity);

            return new Domain.Member(memberEntity.Id, memberEntity.Name ?? string.Empty);
        }

        public IEnumerable<Domain.LibraryItem> FindItems(string? term)
        {
            var entities = _repository.GetAllLibraryItems() ?? Enumerable.Empty<LibraryItem>();
            if (string.IsNullOrWhiteSpace(term)) return entities.Select(e => e.ToDomain());

            var q = term.Trim().ToLowerInvariant();
            return entities
                .Where(e => (e.Title ?? string.Empty).ToLowerInvariant().Contains(q))
                .Select(e => e.ToDomain());
        }
        

        public bool BorrowItem(int memberId, int itemId,  out string message)
        {
            
            var member = _repository.GetMemberById(memberId);
            if (member is null)
            {
                message = "Member not found.";
                return false;
            }
            
            if (member.EndDate.HasValue && DateTime.UtcNow > member.EndDate.Value)
            {
                message = $"{member.Name}'s membership has expired and cannot borrow items.";
                return false;
            }

            var libraryItemEntity = _repository.GetLibraryItemById(itemId);
            if (libraryItemEntity is null)
            {
                message = "Item not found.";
                return false;
            }

            if (libraryItemEntity.IsBorrowed)
            {
                message = $"'{libraryItemEntity.Title}' is already borrowed.";
                return false;
            }

            var borrowItems = _repository.GetAllBorrowedItems() 
                .Where(b => b.MemberId == memberId && b.LibraryItem != null && b.LibraryItem.IsBorrowed);
            var expired = borrowItems.Any(b => b.BorrowedDate.HasValue && DateTime.Now > b.BorrowedDate.Value.AddDays(3));
    
            if (expired)
            {
                message = $"{member.Name} cannot borrow more items until returning expired borrowed items";
                return false;
            }
            
            if (borrowItems.Count() >= 3)
            {
                message = $"{member.Name} cannot borrow more than 3 items at the same time.";
                return false;
            }

            libraryItemEntity.IsBorrowed = true;
            libraryItemEntity.Active = true;
            _repository.UpdateLibraryItem(libraryItemEntity);

            var borrowed = new BorrowedItem { MemberId = memberId, LibraryItemId = itemId, BorrowedDate = DateTime.Now, Active = true};
            _repository.AddBorrowedItem(borrowed);
            
            var borrowedItem = _repository.GetBorrowedItem(memberId, itemId); 
            if (borrowedItem != null)
            {
                if (borrowedItem.BorrowedDate != null)
                {
                    var expirationDate = borrowedItem.BorrowedDate.Value.AddDays(3);
                    if (DateTime.UtcNow > expirationDate)
                    {
                        System.Console.WriteLine("Expired borrowed item");
                    }
                    else
                    {
                        System.Console.WriteLine("This borrowed item isn't expired yet");
                    }
                }
            }

            message = $"'{libraryItemEntity.Title}' borrowed by {member.Name} 'on {borrowed.BorrowedDate:d}, active: {libraryItemEntity.Active}";
            return true;
        }

        public bool ReturnItem(int memberId, int itemId, out string message)
        {
            var member = _repository.GetMemberById(memberId);
            if (member is null)
            {
                message = "Member not found.";
                return false;
            }

            var libraryItemEntity = _repository.GetLibraryItemById(itemId);
            if (libraryItemEntity is null)
            {
                message = "Item not found.";
                return false;
            }

            if (!libraryItemEntity.IsBorrowed)
            {
                message = $"'{libraryItemEntity.Title}' is not currently borrowed.";
                return false;
            }

            libraryItemEntity.IsBorrowed = false;
            libraryItemEntity.Active = true;
            _repository.UpdateLibraryItem(libraryItemEntity);

            // Buscar y remover BorrowedItem relacionado (RETURN)
            var borrowed = _repository.GetBorrowedItem(memberId, itemId);
            if (borrowed != null)
            {
                _repository.RemoveBorrowedItem(borrowed);
            }

            message = $"'{libraryItemEntity.Title}' returned by {member.Name} 'on {DateTime.UtcNow}";
            return true;
        }

        public IEnumerable<Domain.LibraryItem> GetAllLibraryItems()
        {
            var libraryItemsEntities = _repository.GetAllLibraryItems() ?? Enumerable.Empty<LibraryItem>();
            return libraryItemsEntities.Select(e => e.ToDomain());
        }
        
    }
}
