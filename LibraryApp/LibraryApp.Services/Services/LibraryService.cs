using LibraryApp.Application.Abstractions;
using LibraryApp.Domain;
using LibraryApp.Domain.Enums;
using LibraryApp.Domain.Entities;
//adaptar el program cs para implementar este servicio

namespace LibraryApp.Application.Services
{
    public sealed class LibraryService : ILibraryService
    {
        private readonly ILibraryAppRepository _repository;

        public LibraryService(ILibraryAppRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Domain.LibraryItem> GetAllLibraryItems()
        {
            var LibraryItemsEntities = _repository.GetAllLibraryItems();
            return LibraryItemsEntities.Select(MapToDomainModel);
        }

        public Book AddBook(string title, string author, int pages = 0)
        {
            var bookEntity = new Domain.Entities.LibraryItems
            {
                Title = title,
                Author = author,
                Pages = pages,
                Type = (int)LibraryItemTypeEnum.Book,
                IsBorrowed = false
            };

            _repository.AddLibraryItems(bookEntity);

            // Map the entity to the domain model Book and return it
            return new Book(bookEntity.Id, bookEntity.Title, bookEntity.Author ?? "Unknown", bookEntity.Pages ?? 0, bookEntity.IsBorrowed);
        }

        public Magazine AddMagazine(string title, int issueNumber, string publisher)
        {
            var magazineEntity = new Domain.Entities.LibraryItems
            {
                Title = title,
                IssueNumber = issueNumber,
                Publisher = publisher,
                Type = (int)LibraryItemTypeEnum.Magazine,
                IsBorrowed = false
            };

            _repository.AddLibraryItems(magazineEntity);
            return new Magazine(magazineEntity.Id, magazineEntity.Title, magazineEntity.IssueNumber ?? 0, magazineEntity.Publisher ?? "Unknown", magazineEntity.IsBorrowed);
        }

        public bool BorrowItem(int memberId, int itemId, out string message)
        {
            var member = _repository.GetMemberById(memberId);
            var libraryItemEntity = _repository.GetLibraryItemById(itemId);
            if (member is null)
            {
                message = "Member not found.";
                return false;
            }

            //here we are going to validate if the member has an active membership, if not he cannot borrow an item
            if ((DateTime.UtcNow - member.MembershipStartDate).TotalDays > 10)
            {
                message = "Member's membership has expired and cannot borrow items.";
                return false;
            }
            //validate if the member has an expired item, 3 days or more is expired, if he has an expired item he cannot borrow another one
            else
            {
                var borrowedItems = _repository.GetAllBorrowedItems().Where(bi => bi.MemberId == memberId && bi.Active).ToList();
                foreach (var borrowedItem in borrowedItems)
                {
                    if ((DateTime.UtcNow - borrowedItem.BorrowedDate).TotalDays > 3)
                    {
                        message = "Member has expired borrowed items and cannot borrow new ones.";
                        return false;
                    }
                }
            }

            //validate if this member has 3 or more active borrowed items, if so he cannot borrow another one
            var activeBorrowedItemsCount = _repository.GetAllBorrowedItems().Count(bi => bi.MemberId == memberId && bi.Active);
            if (activeBorrowedItemsCount >= 3)
            {
                message = "Member has reached the maximum limit of 3 borrowed items.";
                return false;
            }

            if (libraryItemEntity is null)
            {
                message = "Item not found.";
                return false;
            }

            if (libraryItemEntity.IsBorrowed)
            {
                message = "Item is already borrowed.";
                return false;
            }

            libraryItemEntity.IsBorrowed = true;
            _repository.UpdateLibraryItem(libraryItemEntity);
            _repository.AddBorrowedItem(new Domain.Entities.BorrowedItem { MemberId = memberId, LibraryItemId = itemId, BorrowedDate = DateTime.UtcNow, Active = true });
            message = $"Item '{libraryItemEntity.Title}' borrowed successfully by member '{member.Name}' at ' {DateTime.UtcNow}.";
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
            //here we are going to validate if the item is borrowed by the member
            var libraryItemEntity = _repository.GetLibraryItemById(itemId);
            if (libraryItemEntity is null)
            {
                message = "Item not found.";
                return false;
            }
            if (!libraryItemEntity.IsBorrowed)
            {
                message = "Item is not currently borrowed.";
                return false;
            }
            libraryItemEntity.IsBorrowed = false;

            // set the borrowed item as inactive, but first we validate if the member has borrowed the item
            var borrowedItem = _repository.GetAllBorrowedItems()
                .FirstOrDefault(bi => bi.MemberId == memberId && bi.LibraryItemId == itemId && bi.Active);
            if (borrowedItem is null)
            {
                message = "This member did not borrow this item.";
                return false;
            }
            borrowedItem.Active = false;

            //var borrowedItem = _repository.GetAllBorrowedItems()
            //    .FirstOrDefault(bi => bi.MemberId == memberId && bi.LibraryItemId == itemId && bi.Active);

            _repository.UpdateLibraryItem(libraryItemEntity);
            message = $"Item '{libraryItemEntity.Title}' returned successfully by member '{member.Name}'.";
            return true;


        }

        public IEnumerable<Domain.Entities.Member> GetAllMembers()
        {

            var membersEntities = _repository.GetAllMembers();
            return membersEntities;
        }

        public IEnumerable<LibraryItem> FindItems(string? term)
        {
            // aquí buscamos los items por título usando el repositorio
            var itemsEntities = _repository.GetAllLibraryItems()
                .Where(item => string.IsNullOrEmpty(term) || (item.Title != null && item.Title.Contains(term, StringComparison.OrdinalIgnoreCase)));
            return itemsEntities.Select(MapToDomainModel);
        }



        private LibraryItem MapToDomainModel(Domain.Entities.LibraryItems entity)
        {
            return (LibraryItemTypeEnum)entity.Type switch
            {
                LibraryItemTypeEnum.Book => new Book(entity.Id, entity.Title, entity.Author ?? "Unknown", entity.Pages ?? 0, entity.IsBorrowed),
                LibraryItemTypeEnum.Magazine => new Magazine(entity.Id, entity.Title, entity.IssueNumber ?? 0, entity.Publisher ?? "Unknown", entity.IsBorrowed),
                _ => throw new InvalidOperationException("Unknown library item type.")
            };
        }

        public Domain.Member RegisterMember(string name)
        {
            //here we are going to create a new member and save it to the database
            var memberEntity = new Domain.Entities.Member
            {
                Name = name
            };
            _repository.AddMember(memberEntity);
            return new Domain.Member(memberEntity.Id, memberEntity.Name);

        }

        public IEnumerable<BorrowedItem> GetAllBorrowedItemsByMember(int memberId)
        {
            // we call all borrowed items and filter by member id active borrowed items
            var borrowedItemsByMember = _repository.GetAllBorrowedItems()
                .Where(bi => bi.MemberId == memberId && bi.Active)
                .ToList();
            return borrowedItemsByMember;
        }

        public IEnumerable<BorrowedItem> GetAllBorrowedItems()
        {
            //here we will get all active borrowed items
            var borrowedItems = _repository.GetAllBorrowedItems();
            //here we will return active borrowed items only, based only if borrowed item is active, and we will include the library item details
            return borrowedItems.Where(bi => bi.Active).ToList();
        }

        //here we will return all borrowed items of a member, and set their Active status as falsed, and mark the libraryItems as not borrowed. filtered by member id
        public bool ReturnAllItemsByMember(int memberId, out string message)
        {
            var member = _repository.GetMemberById(memberId);
            if (member is null)
            {
                message = "Member not found.";
                return false;
            }
            var borrowedItems = _repository.GetAllBorrowedItems()
                .Where(bi => bi.MemberId == memberId && bi.Active)
                .ToList();
            foreach (var borrowedItem in borrowedItems)
            {
                var libraryItem = _repository.GetLibraryItemById(borrowedItem.LibraryItemId);
                if (libraryItem != null)
                {
                    libraryItem.IsBorrowed = false;
                    _repository.UpdateLibraryItem(libraryItem);
                    borrowedItem.Active = false;
                    _repository.UpdateBorrowedItem(borrowedItem);
                }
            }

            message = $"All items returned successfully for member '{member.Name}'.";
            return true;
        }
    }
}