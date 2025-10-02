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
                IssueNumber = issueNumber,
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
            var searchedItems = _repository.FindItems(term);

            return searchedItems.Select(MapToDomainModel);
        }

        public bool BorrowItem(int memberId, int itemId, out string message)
        {
            //var member = _members.FirstOrDefault(m => m.Id == memberId);
            //var item = _items.FirstOrDefault(i => i.Id == itemId);

            //if (member is null) { message = "Member not found."; return false; }
            //if (item is null) { message = "Item not found."; return false; }

            //try
            //{
            //    member.BorrowItem(item);
            //    message = $"'{item.Title}' borrowed by {member.Name}.";
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    message = ex.Message;
            //    return false;
            //}
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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

        private LibraryItem MapToDomainModel(Domain.Entities.LibraryItem entity)
        {
            return (LibraryItemTypeEnum)entity.Type switch
            {
                LibraryItemTypeEnum.Book => new Book(entity.Id, entity.Title, entity.Author ?? string.Empty, entity.Pages ?? 0),
                LibraryItemTypeEnum.Magazine => new Magazine(entity.Id, entity.Title, entity.IssueNumber ?? 0, entity.Publisher ?? string.Empty),
                _ => throw new NotImplementedException("Unknown library item type.")
            };
        }

        private Member MapToDomainMember(Domain.Entities.Member entity)
        {
            return new Member(entity.Id, entity.Name);
        }
    }
}
