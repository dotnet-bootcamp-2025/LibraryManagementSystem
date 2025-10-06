
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain;
using LibraryApp.Domain.Enums;
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

        public Book AddBook(string title, string author, int pages = 0)
        {
            throw new NotImplementedException();
        }

        public Magazine AddMagazine(string title, int issueNumber, string publisher)
        {
            throw new NotImplementedException();
        }

        public bool BorrowItem(int memberId, int itemId, out string message)
        {
            var member = _repository.GetMemberById(memberId);
            var libraryItemEntity = _repository.GetLibraryItemById(itemId);
            if (libraryItemEntity == null)
            {
                message = "Item not found.";
                return false;
            }
        }

        public IEnumerable<LibraryItem> FindItems(string? term)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Domain.LibraryItem> GetAllLibraryItems()
        {
            var LibraryItemsEntities = _repository.GetAllLibraryItems();
            return LibraryItemsEntities.Select(MapToDomainModel);
        }

        private LibraryItem MapToDomainModel(Domain.Entities.LibraryItems entity)
        {
            return (LibraryItemTypeEnum)entity.Type switch
            {
                LibraryItemTypeEnum.Book => new Book(entity.Id, entity.Title, entity.Author ?? "Unknown", entity.Pages ?? 0),
                LibraryItemTypeEnum.Magazine => new Magazine(entity.Id, entity.Title, entity.IssueNumber ?? 0, entity.Publisher ?? "Unknown"),
                _ => throw new InvalidOperationException("Unknown library item type.")
            };
        }

        public Member RegisterMember(string name)
        {
            throw new NotImplementedException();
        }

        public bool ReturnItem(int memberId, int itemId, out string message)
        {
            throw new NotImplementedException();
        }

    }
}