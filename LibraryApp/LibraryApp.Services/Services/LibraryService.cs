using LibraryApp.Application.Abstractions;
using LibraryApp.Domain;
using LibraryApp.Domain.Enums;
using LibraryApp.Domain.Entities;


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
            return new Domain.Book(bookEntity.Id, bookEntity.Title, bookEntity.Author, (int)bookEntity.Pages);
        }

        /*public Book AddBook(string title, string author, int pages = 0)
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
            return new Domain.Book(bookEntity.Id, bookEntity.Title, bookEntity.Author);*/


        public Magazine AddMagazine(string title, int issueNumber, string publisher)
        {
            //var mag = new Magazine(_nextItemId++, title, issueNumber, publisher);
            //_items.Add(mag);
            //return mag;
            var magazineEntity = new Domain.Entities.LibraryItem
            {
                Title = title,
                IssueNumber = issueNumber,
                Publisher = publisher,
                Type = (int)LibraryItemTypeEnum.Magazine,
                IsBorrowed = false
            };

            _repository.AddLibraryItem(magazineEntity);

            return new Magazine(
                magazineEntity.Id,
                magazineEntity.Title,
                magazineEntity.IssueNumber ?? 0,
                magazineEntity.Publisher ?? string.Empty);
        }

        // Register a new member
        //public Domain.Member RegisterMember(string name)
        //{
        //    //var member = new Member(_nextMemberId++, name);
        //    //_members.Add(member);
        //    //return member;
        //    var newMember = new Domain.Entities.Member
        //    {
        //        Name = name
        //    };

        //    _repository.AddMember(newMember);
        //    return new Domain.Member(newMember.Id, newMember.Name);
        //}

        // Get all members
        public Domain.Member RegisterMember(string name)
        {
            var newMember = new Domain.Entities.Member
            {
                Name = name
            };

            _repository.AddMember(newMember);
            return new Domain.Member(newMember.Id, newMember.Name);
        }

        public IEnumerable<Domain.Member> AllMembers
        {
            get
            {
                var memberEntities = _repository.GetAllMembers();
                return memberEntities.Select(m => new Domain.Member(m.Id, m.Name));
            }
        }

        IEnumerable<Domain.Member> ILibraryService.GetAllMembers => AllMembers;

        public IEnumerable<Domain.LibraryItem> FindItems(string? term)
        {
            var libraryItemsEntities = _repository.GetAllLibraryItems();
            var items = libraryItemsEntities.Select(MapToDomainModel);

            if (string.IsNullOrWhiteSpace(term)) return items;
            term = term.Trim().ToLowerInvariant();
            return items.Where(i => i.Title.ToLowerInvariant().Contains(term));
        }
        // TODO: Is it a GET or a POST?
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

            var borrowedItem = _repository.GetBorrowedItem(memberId, itemId);
            if (borrowedItem is null)
            {
                message = $"'{libraryItemEntity.Title}' is not currently borrowed by {member.Name}.";
                return false;
            }

            // Mark the item as returned
            libraryItemEntity.IsBorrowed = false;
            _repository.UpdateLibraryItem(libraryItemEntity);

            // Remove the borrowed record
            _repository.RemoveBorrowedItem(borrowedItem);

            message = $"'{libraryItemEntity.Title}' successfully returned by {member.Name}.";
            return true;
        }
        
        public IEnumerable<Domain.LibraryItem> GetAllLibraryItems()
        {
            var libraryItemsEntities = _repository.GetAllLibraryItems();
            return libraryItemsEntities.Select(MapToDomainModel);
        }
        //private Domain.LibraryItem MapToDomainModel(Domain.Entities.LibraryItem entity)
        //{
        //    return (LibraryItemTypeEnum)entity.Type switch
        //    {
        //        LibraryItemTypeEnum.Book => new Book(entity.Id, entity.Title, entity.Author ?? string.Empty, entity.Pages ?? 0),
        //        LibraryItemTypeEnum.Magazine => new Magazine(entity.Id, entity.Title, entity.IssueNumber ?? 0, entity.Publisher ?? string.Empty),
        //        _ => throw new InvalidOperationException("Unknown library item type.")
        //    };

        private Domain.LibraryItem MapToDomainModel(Domain.Entities.LibraryItem entity)
        {
            Domain.LibraryItem domainItem = (LibraryItemTypeEnum)entity.Type switch
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
        
        public IEnumerable<Domain.Member> GetAllMembers()
        {
            var memberEntities = _repository.GetAllMembers();
            return memberEntities.Select(m => new Domain.Member(m.Id, m.Name));
        }
        public IEnumerable<object> GetAllMembersWithBorrowStatus()
        {
            var members = _repository.GetAllMembersWithBorrowStatus();

            return members.Select(m => new
            {
                Id = m.Id,
                Name = m.Name,
                HasBorrowedItems = m.BorrowedItems != null && m.BorrowedItems.Any()
            });
        }
    }
    
}