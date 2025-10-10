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

        #region GET

        public IEnumerable<LibraryItem> FindItems(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                var allEntities = _repository.GetAllLibraryItems();
                return allEntities.Select(MapToDomainModel);
            }

            var searchedEntities = _repository.FindItems(term);
            return searchedEntities.Select(MapToDomainModel);
        }

        public IEnumerable<LibraryItem> GetAllLibraryItems()
        {
            var libraryItemsEntities = _repository.GetAllLibraryItems();

            return libraryItemsEntities.Select(MapToDomainModel);
        }

        public IEnumerable<Member> GetAllMembers()
        {
            var memberEntities = _repository.GetAllMembers();

            return memberEntities.Select(MapToDomainMember);
        }

        public (bool Success, IEnumerable<BorrowedItem> Items) GetMemberBorrowedItems(int memberId, out string message)
        {
            var entityMember = _repository.GetMemberById(memberId);

            if (entityMember is null)
            {
                message = "No member was found.";
                return (false, Enumerable.Empty<BorrowedItem>());
            }

            var entityItems = _repository.GetMemberBorrowedItems(memberId);
            message = entityItems.Any() ? $"Items borrowed by {entityMember.Name}"
                : $"{entityMember.Name} has not borrowed items as of now.";

            return (true, entityItems.Select(MapToDomainBorrowed));
        }

        public (bool Success, Member? Member) GetMemberById(int id, out string message)
        {
            var entityMember = _repository.GetMemberById(id);

            if (entityMember is null) { message = "No member was found."; return (false, null); }

            message = "Member found successfully.";
            return (true, MapToDomainMember(entityMember));
        }

        #endregion GET

        #region ADD

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
            _repository.SaveChanges();

            return new Domain.Book(bookEntity.Id, bookEntity.Title, bookEntity.Author, bookEntity.Pages ?? 0);
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
            _repository.SaveChanges();

            return new Domain.Magazine(magEntity.Id, magEntity.Title, magEntity.IssueNumber ?? 0, magEntity.Publisher);
        }

        public Member RegisterMember(string name)
        {
            var memberEntity = new Domain.Entities.Member
            {
                Name = name,
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                EndDate = DateOnly.FromDateTime(DateTime.Today).AddDays(7)
            };

            _repository.AddMember(memberEntity);
            _repository.SaveChanges();

            return new Domain.Member(memberEntity.Id, memberEntity.Name, memberEntity.StartDate.ToString("MM/dd/yyyy"));
        }

        #endregion ADD

        #region BORROW / RETURN

        public bool BorrowItem(int memberId, int itemId, out string message)
        {
            var entityMember = _repository.GetMemberById(memberId);
            var entityItem = _repository.GetItemById(itemId);

            if (entityMember is null) { message = "Member not found."; return false; }
            if (entityItem is null) { message = "Item not found."; return false; }

            var today = DateOnly.FromDateTime(DateTime.Today);
            if (today > entityMember.EndDate) { message = "This member's suscription is no longer active."; return false; }

            var borrowedItems = _repository.GetMemberBorrowedItems(entityMember.Id);
            if (borrowedItems.Count() >= 3) { message = "Members can not borrow more than 3 items."; return false; }

            var expiredItems = borrowedItems.
                Where(b => today > b.BorrowedDate.AddDays(3)
                && b.IsReturned == false)
                .ToList();
            if (expiredItems.Count > 0) { message = "Members that have overdue items to return can not borrow items."; return false; }

            if (entityItem.IsBorrowed) { message = "Item is already borrowed."; return false; }

            entityItem.IsBorrowed = true;
            _repository.UpdateLibraryItem(entityItem);

            var borrowedItem = new Domain.Entities.BorrowedItem
            {
                MemberId = entityMember.Id,
                LibraryItemId = entityItem.Id,
                IsReturned = false,
                BorrowedDate = DateOnly.FromDateTime(DateTime.Today)
            };
            _repository.AddBorrowedItem(borrowedItem);

            _repository.SaveChanges();

            message = $"Item borrowed successfully by {entityMember.Name}.\n Record Id= {borrowedItem.Id}";
            return true;
        }

        public bool ReturnItem(int memberId, int itemId, out string message)
        {
            var entityMember = _repository.GetMemberById(memberId);
            var entityItem = _repository.GetItemById(itemId);

            if (entityMember is null) { message = "Member not found."; return false; }
            if (entityItem is null) { message = "Item not found."; return false; }
            if (!entityItem.IsBorrowed) { message = "Item is not currently borrowed."; return false; }

            var borrowedItem = _repository.GetBorrowedItem(entityMember.Id, entityItem.Id);
            if (borrowedItem is null) { message = "No record of this item being borrowed by this member exists."; return false; }

            entityItem.IsBorrowed = false;
            _repository.UpdateLibraryItem(entityItem);

            borrowedItem.IsReturned = true;
            _repository.UpdateBorrowedItem(borrowedItem);

            _repository.SaveChanges();

            message = $"Item returned successfully by {entityMember.Name}.";
            return true;
        }

        #endregion BORROW / RETURN

        #region MAPPING

        private LibraryItem MapToDomainModel(Domain.Entities.LibraryItem entity)
        {
            return (LibraryItemTypeEnum)entity.Type switch
            {
                LibraryItemTypeEnum.Book => new Book(entity.Id, entity.Title, entity.Author ?? string.Empty, entity.Pages ?? 0, entity.IsBorrowed),
                LibraryItemTypeEnum.Magazine => new Magazine(entity.Id, entity.Title, entity.IssueNumber ?? 0, entity.Publisher ?? string.Empty, entity.IsBorrowed),
                _ => throw new NotImplementedException("Unknown library item type.")
            };
        }

        private Member MapToDomainMember(Domain.Entities.Member entity)
        {
            return new Member(entity.Id, entity.Name, entity.StartDate.ToString("MM/dd/yyyy"));
        }

        private BorrowedItem MapToDomainBorrowed(Domain.Entities.BorrowedItem entity)
        {
            return new BorrowedItem(entity.Id, entity.MemberId, entity.LibraryItemId, entity.IsReturned!.Value, entity.BorrowedDate.ToString("MM/dd/yyyy"));
        }

        #endregion MAPPING
    }
}
