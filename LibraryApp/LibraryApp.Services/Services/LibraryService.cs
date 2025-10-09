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
            return new Book(bookEntity.Id, bookEntity.Title, bookEntity.Author ?? "Unknown", bookEntity.Pages ?? 0);
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
            return new Magazine(magazineEntity.Id, magazineEntity.Title, magazineEntity.IssueNumber ?? 0, magazineEntity.Publisher ?? "Unknown");
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
                message = "Item is already borrowed.";
                return false;
            }

            libraryItemEntity.IsBorrowed = true;
            _repository.UpdateLibraryItem(libraryItemEntity);
            _repository.AddBorrowedItem(new Domain.Entities.BorrowedItem {MemberId = memberId, LibraryItemId = itemId});
            message = $"Item '{libraryItemEntity.Title}' borrowed successfully by member '{member.Name}'.";
            return true;
        }

        public bool ReturnItem(int memberId, int itemId, out string message)
        {
            var member = _repository.GetMemberById(memberId);
            if ( member is null)
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
            _repository.UpdateLibraryItem(libraryItemEntity);
            _repository.AddBorrowedItem(new Domain.Entities.BorrowedItem { MemberId = memberId, LibraryItemId = itemId });
            message = $"Item '{libraryItemEntity.Title}' returned successfully by member '{member.Name}'.";
            return true;


        }

        public IEnumerable<Domain.Member> GetAllMembers()
        {
            var membersEntities = _repository.GetAllMembers();
            return membersEntities.Select(m => new Domain.Member(m.Id, m.Name));
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
                LibraryItemTypeEnum.Book => new Book(entity.Id, entity.Title, entity.Author ?? "Unknown", entity.Pages ?? 0),
                LibraryItemTypeEnum.Magazine => new Magazine(entity.Id, entity.Title, entity.IssueNumber ?? 0, entity.Publisher ?? "Unknown"),
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




    }
}