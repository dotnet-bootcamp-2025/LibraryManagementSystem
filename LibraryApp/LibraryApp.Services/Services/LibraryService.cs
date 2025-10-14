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

            if (bookEntity.Pages > 100)
                throw new ArgumentException("A book cannot have more than 100 pages.");


            _repository.AddLibraryItem(bookEntity);
            return new Domain.Book(bookEntity.Id, bookEntity.Title, bookEntity.Author, (int)bookEntity.Pages);
        }

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
        //Method to test RegisterMember with email
        //public Member RegisterMember(string name, string email)
        //{
        //    var member = new Member { Name = name, Email = email };
        //    _repository.RegisterMember(member);
        //    return member;
        //}

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

            // Validate membership expiration
            if (DateTime.Now > member.EndDate)
            {
                message = $"Membership for {member.Name} has expired on {member.EndDate:MM/dd/yyyy}. Renewal required.";
                return false;
            }

            // Validate borrow limit
            var activeBorrows = member.BorrowedItems?
                .Count(b => b.LibraryItem != null && b.LibraryItem.Active == false) ?? 0;

            if (activeBorrows >= 3)
            {
                message = $"Member {member.Name} already has 3 borrowed items. Return one before borrowing another.";
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

            // Borrow date logic
            var borrowDate = DateTime.Now;
            var expirationDate = borrowDate.AddDays(3);

            var borrowedItem = new Domain.Entities.BorrowedItem
            {
                MemberId = memberId,
                LibraryItemId = itemId,
                BorrowDate = borrowDate,
                ExpirationDate = expirationDate
            };

            // Mark item inactive (soft delete)
            libraryItemEntity.IsBorrowed = true;
            libraryItemEntity.Active = false;

            _repository.UpdateLibraryItem(libraryItemEntity);
            _repository.AddBorrowedItem(borrowedItem);

            message = $"'{libraryItemEntity.Title}' borrowed by {member.Name}. Expiration: {expirationDate:MM/dd/yyyy}";
            return true;
        //    // 🔒 Check for expired borrowed items before allowing new borrow
        //    var expiredBorrow = member.BorrowedItems?.Any(b =>
        //        b.ExpirationDate < DateTime.Now && b.LibraryItem != null && b.LibraryItem.IsBorrowed) ?? false;

        //    if (expiredBorrow)
        //    {
        //        message = $"Member {member.Name} has an expired borrowed item. Cannot borrow a new one.";
        //        return false;
        //    }

        //    var libraryItemEntity = _repository.GetLibraryItemById(itemId);
        //    if (libraryItemEntity is null)
        //    {
        //        message = "Item not found.";
        //        return false;
        //    }

        //    if (libraryItemEntity.IsBorrowed)
        //    {
        //        message = $"'{libraryItemEntity.Title}' is already borrowed.";
        //        return false;
        //    }

        //    // ✅ Borrow date + expiration
        //    var borrowDate = DateTime.Now;
        //    var expirationDate = borrowDate.AddDays(3);

        //    var borrowedItem = new Domain.Entities.BorrowedItem
        //    {
        //        MemberId = memberId,
        //        LibraryItemId = itemId,
        //        BorrowDate = borrowDate,
        //        ExpirationDate = expirationDate
        //    };

        //    // Soft delete effect — mark item inactive

        //    libraryItemEntity.IsBorrowed = true;
        //    libraryItemEntity.Active = false;
        //    _repository.UpdateLibraryItem(libraryItemEntity);
        //    _repository.AddBorrowedItem(borrowedItem);

        //    message = $"'{libraryItemEntity.Title}' borrowed by {member.Name}. Expiration: {expirationDate:MM/dd/yyyy}";
        //    return true;
        //
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

            var borrowedItem = _repository.GetBorrowedItem(memberId, itemId);
            if (borrowedItem is null)
            {
                message = $"'{libraryItemEntity.Title}' is not currently borrowed by {member.Name}.";
                return false;
            }

            // Reactivate the item (soft undelete)
            libraryItemEntity.IsBorrowed = false;
            libraryItemEntity.Active = true;

            _repository.UpdateLibraryItem(libraryItemEntity);
            _repository.RemoveBorrowedItem(borrowedItem);

            message = $"'{libraryItemEntity.Title}' successfully returned by {member.Name}.";
            return true;
        }

        public IEnumerable<Domain.LibraryItem> GetAllLibraryItems()
        {
            var libraryItemsEntities = _repository.GetAllLibraryItems();
            return libraryItemsEntities.Select(MapToDomainModel);
        }

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
                StartDate = m.StartDate.ToString("MM/dd/yyyy"),
                EndDate = m.EndDate.ToString("MM/dd/yyyy"),
                MembershipStatus = DateTime.Now <= m.EndDate ? "Active" : "Expired",
                HasBorrowedItems = m.BorrowedItems != null && m.BorrowedItems.Any(),
                BorrowedItems = m.BorrowedItems?.Select(b => new
                {
                    b.LibraryItemId,
                    ItemTitle = b.LibraryItem?.Title,
                    BorrowDate = b.BorrowDate.ToString("MM/dd/yyyy"),
                    ExpirationDate = b.ExpirationDate.ToString("MM/dd/yyyy"),
                    Status = b.ExpirationDate < DateTime.Now ? "Expired borrow item" : "Active"

                    // Borrowed Items 1st refactor
                    //Id = m.Id,
                    //Name = m.Name,
                    //HasBorrowedItems = m.BorrowedItems != null && m.BorrowedItems.Any(),
                    //BorrowedItems = m.BorrowedItems?.Select(b => new
                    //{
                    //    b.LibraryItemId,
                    //    ItemTitle = b.LibraryItem?.Title,
                    //    BorrowDate = b.BorrowDate.ToString("MM/dd/yyyy"),
                    //    ExpirationDate = b.ExpirationDate.ToString("MM/dd/yyyy"),
                    //    Status = b.IsExpired ? "Expired borrow item" : "Active"
                })
            });
        }

        public IEnumerable<object> GetBorrowedItemsByMemberId(int memberId)
        {
            var member = _repository.GetMemberById(memberId);
            if (member is null)
                throw new InvalidOperationException($"Member with ID {memberId} not found.");

            // Get only active borrowed items (soft delete respected)
            var borrowedItems = member.BorrowedItems?
                .Where(b => b.LibraryItem != null && b.LibraryItem.Active == false)
                .Select(b => new
                {
                    LibraryItemId = b.LibraryItemId,
                    Title = b.LibraryItem?.Title,
                    BorrowDate = b.BorrowDate.ToString("MM/dd/yyyy"),
                    ExpirationDate = b.ExpirationDate.ToString("MM/dd/yyyy"),
                    Status = b.ExpirationDate < DateTime.Now
                        ? "Expired borrow item"
                        : "Active"
                });

            return borrowedItems?.Cast<object>().ToList() ?? new List<object>();
        }
    }
}