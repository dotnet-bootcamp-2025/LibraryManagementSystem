using LibraryApp.Application.Abstractions;
using LibraryApp.Domain;
using LibraryApp.Domain.Enum;

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
            var bookEntity = new Domain.Entities.LibraryItem
            {
                Title = title,
                Author = author,
                Pages = pages,
                Type = (int)LibraryItemTypeEnum.Book,
                IsBorrowed = false
            };
            _repository.AddLibraryItem(bookEntity);
            return new Domain.Book(bookEntity.Id, bookEntity.Title, bookEntity.Author, (int)bookEntity.Pages, bookEntity.IsBorrowed);
        }
        public Magazine AddMagazine(string title, int issueNumber, string publisher)
        {
            var magEntity = new Domain.Entities.LibraryItem
            {
                Title = title,
                IssueNumber = issueNumber,
                Publisher = publisher,
                Type = (int)LibraryItemTypeEnum.Magazine,
                IsBorrowed = false
            };
            _repository.AddLibraryItem(magEntity);
            return new Domain.Magazine(magEntity.Id, magEntity.Title, (int)magEntity.IssueNumber, magEntity.Publisher, magEntity.IsBorrowed);
        }
        public Domain.Member RegisterMember(string name)
        {
            var memberEntity = new Domain.Entities.Member
            {
                Name = name
                //BorrowedItems = null
            };
            _repository.AddMember(memberEntity);
            return new Domain.Member(memberEntity.Id, memberEntity.Name);
        }
        public IEnumerable<Domain.LibraryItem> FindItems(string? term)
        {
            //if (string.IsNullOrWhiteSpace(term)) return _items;
            //term = term.Trim().ToLowerInvariant();
            //return _items.Where(i => i.Title.ToLowerInvariant().Contains(term));
            throw new NotImplementedException();
        }

        // TODO: Is it a GET or a POST? / POST to have more tracking (dates, penalties) or PUT/PATCH to change Member and Item
        public bool BorrowItem(int memberId, int itemId, out string message)
        {
            var member = _repository.GetMemberById(memberId);
            if(member is null)
            {
                message = "Member not found.";
                return false;
            }
            var libraryItemEntity = _repository.GetLibraryItemById(itemId);
            if(libraryItemEntity is null)
            {
                message = "Item not found.";
                return false;
            }
            var borrowedItems = _repository.GetBorrowedItemsFromMember(memberId);
            if(borrowedItems.Count() >= 3)
            {
                message = $"{member.Name} has already borrowed 3 items. Return some items before borrowing more.";
                return false;
            }
            foreach(var borrowed in borrowedItems)
            {
                if(borrowed.BorrowDate <= DateTime.UtcNow)
                {
                    message = $"{member.Name} cannot borrow because '{libraryItemEntity.Title}' is expired. Return it before borrowing again.";
                    return false;
                }
            }
            if (libraryItemEntity.IsBorrowed)
            {
                message = $"'{libraryItemEntity.Title}' is already borrowed.";
                return false;
            }
            libraryItemEntity.IsBorrowed = true;
            
            _repository.UpdateLibraryItem(libraryItemEntity);
            _repository.AddBorrowedItem(new Domain.Entities.BorrowedItem { MemberId = memberId, LibraryItemId = itemId});

            message = $"'{libraryItemEntity.Title}' borrowed by {member.Name}.";
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
                message = $"'{libraryItemEntity.Title}' is not borrowed yet.";
                return false;
            }
            libraryItemEntity.IsBorrowed = false;

            _repository.UpdateLibraryItem(libraryItemEntity);
            _repository.ReturnBorrowedItem(new Domain.Entities.BorrowedItem { MemberId = memberId, LibraryItemId = itemId });

            message = $"' {member.Name} has returned '{libraryItemEntity.Title}'.";
            return true;
        }

        public IEnumerable<Domain.LibraryItem> GetAllLibraryItems()
        {
            var libraryItemsEntities = _repository.GetAllLibraryItems();
            return libraryItemsEntities.Select(MapToDomainModelItem);
        }
        public IEnumerable<Domain.Member> GetAllMembers()
        {
            var memberEntities = _repository.GetAllMembers();
            return memberEntities.Select(MapToDomainModelMember);
        }
        public IEnumerable<Domain.BorrowedItem> GetBorrowedItemsFromMember(int memberId)
        {
            var borrowedEntities = _repository.GetBorrowedItemsFromMember(memberId);
            return borrowedEntities.Select(MapToDomainModelBorrowed);
        }

        private Domain.LibraryItem MapToDomainModelItem(Domain.Entities.LibraryItem entity)
        {
            return (LibraryItemTypeEnum)entity.Type switch
            {
                LibraryItemTypeEnum.Book => new Book(entity.Id, entity.Title, entity.Author!, entity.Pages ?? 0, entity.IsBorrowed),
                LibraryItemTypeEnum.Magazine => new Magazine(entity.Id, entity.Title, entity.IssueNumber ?? 0, entity.Publisher!, entity.IsBorrowed),
                _ => throw new InvalidOperationException("Unknown library item type.")
            };
        }

        private Domain.Member MapToDomainModelMember(Domain.Entities.Member entity)
        {
            return new Domain.Member(entity.Id, entity.Name);
        }
        private Domain.BorrowedItem MapToDomainModelBorrowed(Domain.Entities.BorrowedItem entity)
        {
            return new Domain.BorrowedItem(entity.Id, entity.MemberId, entity.LibraryItemId, entity.BorrowDate, entity.Active);
        }
    }
}
