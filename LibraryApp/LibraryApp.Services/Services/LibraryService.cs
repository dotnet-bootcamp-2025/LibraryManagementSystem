using LibraryApp.Application.Abstractions;
using LibraryApp.Domain;
using LibraryApp.Domain.Entities;
using LibraryApp.Domain.Enums;
using System.Globalization;

namespace LibraryApp.Application.Services
{
    public sealed class LibraryService : ILibraryService
    {
        private readonly ILibraryAppRepository _repository;
        public LibraryService(ILibraryAppRepository repository)
        {
            _repository = repository;
        }

        // Interface Implementation
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
                message = $"'{libraryItemEntity.Title}' is already borrowed.";
                return false;
            }

            // TODO: Block more than 3 borrows by the same Member
            IEnumerable<BorrowedItem> borrowedItemsByMember = _repository.GetBorrowedItemsByMember(member.Id);

            if (borrowedItemsByMember.Count() > 0)
            {
                // CHeck for Overdue Items
                foreach(BorrowedItem bi  in borrowedItemsByMember)
                {
                    if(bi.ReturnDeadLine < DateTime.Now)
                    {
                        var overdueItem = _repository.GetLibraryItem(bi.LibraryItemId);
                        message = $"Not allowed to borrow more Items for {member.Name} until {overdueItem.Title} is returned. Deadline was {bi.ReturnDeadLine.ToString("d", new CultureInfo("en-us"))}";
                        return false;
                    }
                }

                if (borrowedItemsByMember.Count() >= 3)
                {
                    message = $"Not allowed to borrow more than 3 Items. Please return another Item and try again.";
                    return false;
                }
            }



            // TODO: Set ReturnDeadLine by today + 3 days
            DateTime today = DateTime.Now;
            DateTime deadLine = new DateTime(
                today.Year
                , today.Month
                , today.Day
                , 23
                , 59
                , 59
                ).AddDays(3);

            _repository.AddBorrowedItem(new Domain.Entities.BorrowedItem { MemberId = memberId, LibraryItemId = itemId, ReturnDeadLine = deadLine, Active = true });
            libraryItemEntity.IsBorrowed = true;
            _repository.UpdateLibraryItem(libraryItemEntity);

            message = $"'{libraryItemEntity.Title}' borrowed by {member.Name}. Must be returned by {deadLine}";

            return true;
        }
        public IEnumerable<Domain.LibraryItem> FindItems(string? term)
        {
            throw new NotImplementedException();
        }
        public Domain.Member RegisterMember(string name)
        {
            var memberEntity = new Domain.Entities.Member
            {
                Name = name,
                StartDate = DateTime.Now,
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 0).AddMonths(3)
            };

            _repository.AddMember(memberEntity);

            return new Domain.Member(memberEntity.Id, memberEntity.Name, memberEntity.StartDate, memberEntity.EndDate);
        }
        public bool ReturnItem(int memberId, int itemId, out string message)
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
            // TODO: Modify ReturnBorrowedItem for a Soft Delete
            _repository.ReturnBorrowedItem(borrowedItemEntity.Id);

            message = $"'{libraryItemEntity.Title}' returned by {member.Name}.";

            return true;
        }
        public IEnumerable<Domain.LibraryItem> GetAllLibraryItems()
        {
            var libraryItemsEntities = _repository.GetAllLibraryItems();
            return libraryItemsEntities.Select(MapToDomainModel);
        }
        public IEnumerable<Domain.LibraryItem> GetAllLibraryItemsByMemberId(int memberId)
        {
            var libraryItemsEntities = _repository.GetAllLibraryItemsByMemberId(memberId);
            return libraryItemsEntities.Select(MapToDomainModel);
        }
        public IEnumerable<Domain.Member> GetAllMembers()
        {
            var membersEntities = _repository.GetAllMembers();
            return membersEntities.Select(MapToDomainMembersModel);
        }

        // Private Methods
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
        private Domain.Member MapToDomainMembersModel(Domain.Entities.Member entity)
        {
            return new Domain.Member(entity.Id, entity.Name, entity.StartDate, entity.EndDate);

            // Unfinished attemp for returning BorrowedItems
            var member = new Domain.Member(entity.Id, entity.Name, entity.StartDate, entity.EndDate);
            foreach(var itemId in entity.BorrowedItems)
            {

            }
        }
    }
}
