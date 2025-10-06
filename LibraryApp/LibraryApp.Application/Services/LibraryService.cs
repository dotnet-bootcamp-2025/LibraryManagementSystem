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
                IsBorrowed = false
            };
            _repository.AddLibraryItem(bookEntity);

            return new Book(bookEntity.Id, bookEntity.Title ?? string.Empty, bookEntity.Author ?? string.Empty, bookEntity.Pages ?? 0);
        }

        public Magazine AddMagazine(string title, int issueNumber, string publisher)
        {
            var magEntity = new LibraryItem
            {
                Title = title,
                IssueNumber = issueNumber,
                Publisher = publisher,
                Type = (int)LibraryItemTypeEnum.Magazine,
                IsBorrowed = false
            };
            _repository.AddLibraryItem(magEntity);
            return new Magazine(magEntity.Id, magEntity.Title ?? string.Empty, magEntity.IssueNumber ?? 0, magEntity.Publisher ?? string.Empty);
        }
        
        public IEnumerable<Domain.Member> GetAllMembers()
        {
            var entities = _repository.GetAllMembers() ?? Enumerable.Empty<Domain.Entities.Member>();
            // Mapear entidad -> modelo de dominio (aquí asumo Domain.Member tiene ctor (int id, string name))
            return entities.Select(e => new Domain.Member(e.Id, e.Name ?? string.Empty));
        }


        public Domain.Member RegisterMember(string name)
        {
            var memberEntity = new Member { Name = name };
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

            var borrowed = new BorrowedItem { MemberId = memberId, LibraryItemId = itemId };
            _repository.AddBorrowedItem(borrowed);

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
                message = $"'{libraryItemEntity.Title}' is not currently borrowed.";
                return false;
            }

            libraryItemEntity.IsBorrowed = false;
            _repository.UpdateLibraryItem(libraryItemEntity);

            // Buscar y remover BorrowedItem relacionado
            var borrowed = _repository.GetBorrowedItem(memberId, itemId);
            if (borrowed != null)
            {
                _repository.RemoveBorrowedItem(borrowed);
            }

            message = $"'{libraryItemEntity.Title}' returned by {member.Name}.";
            return true;
        }

        public IEnumerable<Domain.LibraryItem> GetAllLibraryItems()
        {
            var libraryItemsEntities = _repository.GetAllLibraryItems() ?? Enumerable.Empty<LibraryItem>();
            return libraryItemsEntities.Select(e => e.ToDomain());
        }
    }
}
