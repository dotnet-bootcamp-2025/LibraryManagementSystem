using System.Reflection.Metadata;
using LibraryApp.Application.Abstraction;
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
            var magazineEntity = new Domain.Entities.LibraryItem
            {
                Title = title,
                IssueNumber = issueNumber,
                Publisher = publisher,
                Type = (int)LibraryItemTypeEnum.Magazine,
                IsBorrowed = false
            };
            _repository.AddLibraryItem(magazineEntity);
            return new Domain.Magazine(magazineEntity.Id, magazineEntity.Title, magazineEntity.IssueNumber ?? 0, magazineEntity.Publisher ?? string.Empty);
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
            if (libraryItemEntity.IsBorrowed)
            {
                message = $"'{libraryItemEntity.Title}' is already borrowed.";
                return false;
            }
            libraryItemEntity.IsBorrowed = true;
            _repository.UpdateLibraryItem(libraryItemEntity);
            _repository.AddBorrowedItem(new Domain.Entities.BorrowedItem { MemberId = memberId, LibraryItemId = itemId });
            message = $"'{libraryItemEntity.Title}' borrowed by {member.Name}.";
            return true;
        }


        public bool ReturnItem(int memberId, int itemId)
        {
            var member = _repository.GetMemberById(memberId);
            if (member == null)
                return false;

            var libraryItemEntity = _repository.GetLibraryItemById(itemId);
            if (libraryItemEntity is null || !libraryItemEntity.IsBorrowed)
                return false;

            var borrowedItem = member.BorrowedItems?
                .FirstOrDefault(bi => bi.LibraryItemId == itemId);

            if (borrowedItem == null)
                return false;

            libraryItemEntity.IsBorrowed = false;

            member.BorrowedItems!.Remove(borrowedItem);

            _repository.UpdateLibraryItem(libraryItemEntity);
            _repository.UpdateMember(member);

            return true;
        }
    
        public IEnumerable<LibraryItem> GetAllLibraryItems()
        {
            var libraryItemsEntities = _repository.GetAllLibraryItems();

            return libraryItemsEntities.Select(MapToDomainModel);
        }

        private LibraryItem MapToDomainModel(Domain.Entities.LibraryItem entity)
        {

            LibraryItem domainItem = (LibraryItemTypeEnum)entity.Type switch
            {
                LibraryItemTypeEnum.Book => new Book(
                    entity.Id, entity.Title,
                    entity.Author ??
                    string.Empty,
                    entity.Pages ?? 0),
                LibraryItemTypeEnum.Magazine => new Magazine(
                    entity.Id,
                    entity.Title,
                    entity.IssueNumber ?? 0,
                    entity.Publisher ??
                    string.Empty),
                _ => throw new NotSupportedException($"Library item type '{entity.Type}' is not supported.")
            };
            domainItem.IsBorrowed = entity.IsBorrowed;
            return domainItem;
        }

        IEnumerable<Member> ILibraryService.GetAllMembers()
        {
            var memberEntities = _repository.GetAllMembers();

            return memberEntities.Select(entity => {
                var borrowedDomainItems = entity.BorrowedItems
                    .Where(borrowed => borrowed.LibraryItem != null && borrowed.LibraryItem.IsBorrowed)
                    .GroupBy(borrowed => borrowed.LibraryItem!.Id)
                    .Select(group => group.First())
                    .Select(borrowed => MapToDomainModel(borrowed.LibraryItem!))
                    .ToList();

                return new Member(entity.Id, entity.Name, borrowedDomainItems);
            });
        }
    }
}
