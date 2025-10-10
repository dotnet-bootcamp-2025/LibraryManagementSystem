using LibraryApp.Application.Abstraction;
using LibraryApp.Domain;
using LibraryApp.Domain.Enums;
using LibraryApp.Application.DTOs;

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

            _repository.SaveChanges();

            return new Domain.Book(bookEntity.Id, bookEntity.Title, bookEntity.Author, bookEntity.Pages ?? 0);
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
            _repository.SaveChanges();
            return new Domain.Magazine(magazineEntity.Id, magazineEntity.Title, magazineEntity.IssueNumber ?? 0, magazineEntity.Publisher ?? string.Empty);
        }

        public Member RegisterMember(string name)
        {
            var memberEntity = new Domain.Entities.Member
            {
                Name = name
            };
            _repository.AddMember(memberEntity);
            _repository.SaveChanges();
            return new Domain.Member(memberEntity.Id, memberEntity.Name);
        }

        public IEnumerable<LibraryItem> FindItems(string? term)
        {
            var items = _repository.FindItems(term!);
            return items.Select(MapToDomainModel);
        }

        public bool BorrowItem(int memberId, int itemId, out string message, out string? formattedReturnDate)
        {
            var now = DateTime.UtcNow;

            var member = _repository.GetMemberById(memberId);
            if (member is null)
            {
                message = "Member not found.";
                formattedReturnDate = null;
                return false;
            }

            if (member.BorrowedItems != null && member.BorrowedItems.Count(borrowedItem => borrowedItem.IsActive) >= 3)
            {
                message = "Cannot borrow more than 3 items.";
                formattedReturnDate = null;
                return false;
            }

            var libraryItemEntity = _repository.GetLibraryItemById(itemId);
            if (libraryItemEntity is null)
            {
                message = "Item not found.";
                formattedReturnDate = null;
                return false;
            }

            if (member.BorrowedItems != null && member.BorrowedItems.Any(bi => bi.ReturnDate < now))
            {
                message = "Cannot borrow new items. The member has expired items that must be returned first.";
                formattedReturnDate = null;
                return false;
            }

            if (libraryItemEntity.IsBorrowed)
            {
                var borrowedInfo = _repository.GetBorrowedItemByLibraryItemId(itemId);

                if (borrowedInfo != null)
                {
                    if (borrowedInfo.ReturnDate < now)
                    {
                        message = $"'{libraryItemEntity.Title}' is already borrowed and its return is overdue.";
                    }
                    else
                    {
                        TimeSpan timeRemaining = borrowedInfo.ReturnDate - now;
                        message = $"'{libraryItemEntity.Title}' is currently borrowed, {timeRemaining.Days} days left for its return.";
                    }
                }
                else
                {
                    message = $"'{libraryItemEntity.Title}' is already borrowed, Sorry";
                }

                formattedReturnDate = null;
                return false;
            }

            libraryItemEntity.IsBorrowed = true;
            _repository.UpdateLibraryItem(libraryItemEntity);

            var calculatedReturnDate = now.AddDays(3);
            formattedReturnDate = calculatedReturnDate.ToString("MM/dd/yyyy");

            _repository.AddBorrowedItem(new Domain.Entities.BorrowedItem
            {
                MemberId = memberId,
                LibraryItemId = itemId,
                BorrowedDate = now,
                ReturnDate = calculatedReturnDate,
                IsActive = true
            });

            _repository.SaveChanges();

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
                .FirstOrDefault(bi => bi.LibraryItemId == itemId && bi.IsActive);

            if (borrowedItem is null)
                return false;

            libraryItemEntity.IsBorrowed = false;
            borrowedItem.IsActive = false;

            _repository.UpdateBorrowedItem(borrowedItem);
            _repository.SaveChanges();

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

            return memberEntities.Select(entity => new Member(entity.Id, entity.Name));
        }

        public IEnumerable<LoanDetailsDto> GetMemberActiveLoans(int memberId)
        {
            var ActiveLoans = _repository.GetActiveLoansByMemberId(memberId);

            return ActiveLoans.Select(loan => new LoanDetailsDto
            {
                ItemTitle = loan.LibraryItem!.Title,
                BorrowedDate = loan.BorrowedDate,
                ReturnDate = loan.ReturnDate,
                IsExpired = loan.ReturnDate < DateTime.UtcNow
            }).ToList();
        }

        public bool MemberExists(int memberId)
        {
            var member = _repository.GetMemberById(memberId);
            return member != null;
        }
    }
}