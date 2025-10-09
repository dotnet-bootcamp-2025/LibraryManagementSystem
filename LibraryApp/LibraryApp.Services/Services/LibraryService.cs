using LibraryApp.Application.Abstractions;
using LibraryApp.Domain;
using LibraryApp.Domain.Enums;

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

            return new Domain.Book(bookEntity.Id, bookEntity.Title, bookEntity.Author);
        }
        public Magazine AddMagazine(string title, int issueNumber, string publisher)
        {
            var magEntity = new Domain.Entities.LibraryItem
            {
                Title = title,
                IssueNumber = (int)issueNumber,
                Publisher = publisher,
                Type = (int)LibraryItemTypeEnum.Magazine,
                IsBorrowed = false
            };

            _repository.AddLibraryItem(magEntity);

            return new Domain.Magazine(magEntity.Id, magEntity.Title, magEntity.IssueNumber ?? 0, magEntity.Publisher);
        }
        public Member RegisterMember(string name)
        {
            var memberEntity = new Domain.Entities.Member
            {
                Name = name
            };

            _repository.AddMember(memberEntity);

            return new Domain.Member(memberEntity.Id, memberEntity.Name);
        }
        public IEnumerable<LibraryItem> FindItems(string? term)
        {
            //if (string.IsNullOrWhiteSpace(term)) return _items;
            //term = term.Trim().ToLowerInvariant();
            //return _items.Where(i => i.Title.ToLowerInvariant().Contains(term));
            throw new NotImplementedException();
        }
        public bool BorrowItem(int memberId, int itemId, out string message)
        {
            var member = _repository.GetMemberById(memberId);

            if (member == null)
            {
                message = "Member not found";
                return false;
            }

            var libraryItemEntity = _repository.GetLibraryItem(itemId);

            if (libraryItemEntity == null)
            {
                message = "Item not found";
                return false;
            }

            if (libraryItemEntity.IsBorrowed)
            {
                message = $"{libraryItemEntity.Title}' is already borrowed.";
                return false;
            }

            libraryItemEntity.IsBorrowed = true;

            _repository.UpdateLibraryItem(libraryItemEntity);
            _repository.AddBorrowedItem(new Domain.Entities.BorrowedItem { MemberId = memberId, LibraryItemId = itemId });

            message = $"'{libraryItemEntity.Title}' borrowed by {member.Name}.";

            return true;
        }
        public bool ReturnItem(int memberId, int itemId, out string message)
        {
            //var member = _members.FirstOrDefault(m => m.Id == memberId);
            //var item = _items.FirstOrDefault(i => i.Id == itemId);
            //if (member is null) { message = "Member not found."; return false; }
            //if (item is null) { message = "Item not found."; return false; }
            //try
            //{
            //    member.ReturnItem(item);
            //    message = $"'{item.Title}' returned by {member.Name}.";
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    message = ex.Message;
            //    return false;
            //}

            var member = _repository.GetMemberById(memberId);

            if (member == null)
            {
                message = "Member not found";
                return false;
            }

            var libraryItemEntity = _repository.GetLibraryItem(itemId);

            if (libraryItemEntity == null)
            {
                message = "Item not found";
                return false;
            }

            if (!libraryItemEntity.IsBorrowed)
            {
                message = $"{libraryItemEntity.Title}' isn't borrowed.";
                return false;
            }

            var borrowedItemEntity = _repository.GetBorrowedItem(member.Id, libraryItemEntity.Id);

            if (borrowedItemEntity == null)
            {
                message = "Item borrowed by someone else";
                return false;
            }

            libraryItemEntity.IsBorrowed = false;

            _repository.UpdateLibraryItem(libraryItemEntity);
            _repository.ReturnBorrowedItem(borrowedItemEntity.Id);

            message = $"'{libraryItemEntity.Title}' returned by {member.Name}.";

            return true;
        }

        public IEnumerable<Domain.LibraryItem> GetAllLibraryItems()
        {
            var libraryItemsEntities = _repository.GetAllLibraryItems();
            return libraryItemsEntities.Select(MapToDomainModel);
        }

        private Domain.LibraryItem MapToDomainModel(Domain.Entities.LibraryItem entity)
        {
            if (entity.IsBorrowed) {
                switch ((LibraryItemTypeEnum)entity.Type)
                {
                    case LibraryItemTypeEnum.Book:
                        var book = new Book(entity.Id, entity.Title, entity.Author ?? string.Empty, entity.Pages ?? 0);
                        book.Borrow();
                        return book;
                    case LibraryItemTypeEnum.Magazine:
                        var mag = new Magazine(entity.Id, entity.Title, entity.IssueNumber ?? 0, entity.Publisher ?? string.Empty);
                        mag.Borrow();
                        return mag;
                    default:
                        throw new InvalidOperationException("Unknown library item type.");

                }
            }

            return (LibraryItemTypeEnum)entity.Type switch
            {
                LibraryItemTypeEnum.Book => new Book(entity.Id, entity.Title, entity.Author ?? string.Empty, entity.Pages ?? 0),
                LibraryItemTypeEnum.Magazine => new Magazine(entity.Id, entity.Title, entity.IssueNumber ?? 0, entity.Publisher ?? string.Empty),
                _ => throw new InvalidOperationException("Unknown library item type.")
            };
        }

        public IEnumerable<Domain.Member> GetAllMembers()
        {
            var membersEntities = _repository.GetAllMembers();
            return membersEntities.Select(MapToDomainMembersModel);
        }

        private Domain.Member MapToDomainMembersModel(Domain.Entities.Member entity)
        {
            return new Member(entity.Id, entity.Name);

            // Unfinished attemp for returning BorrowedItems
            var member = new Member(entity.Id, entity.Name);
            foreach(var itemId in entity.BorrowedItems)
            {

            }
        }
    }
}
