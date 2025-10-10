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
        public IEnumerable<Member> GetAllMembers()
        {
            var membersEntities = _repository.GetAllMembers();
            return membersEntities.Select(MapMemberToDomainModel);
        }
        public Member RegisterMember(string name)
        {
            var startDate = DateTime.Now;
            var endDate = startDate.AddYears(1);
            var memberEntity = new Domain.Entities.Member
            {
                Name = name,
                MembershipStartDate = startDate,
                MembershipEndDate = endDate
            };
            _repository.RegisterMember(memberEntity);
            return new Domain.Member(memberEntity.Id, memberEntity.Name);
        }
        public IEnumerable<LibraryItem> FindItems(string? term)
        {
            //if i search anything related to title, author, publisher, it should return all results
            var itemsEntities = _repository.FindItems(term);
            return itemsEntities.Select(MapToDomainModel);
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

            // Check 3 items max limit per member 
            var activeBorrowedItems = _repository.GetActiveBorrowedItemsByMember(memberId).Count();
            if (activeBorrowedItems >= 3)
            {
                message = $"Member {member.Name} has reached the maximum limit of 3 borrowed items.";
                return false;
            }

            //Check if member has an expired borrow item
            var allMemberBorrowedItems = _repository.GetActiveBorrowedItemsByMember(memberId);
            var expiredItems = allMemberBorrowedItems.Where(bi => bi.DueDate < DateTime.Now).ToList();
            if (expiredItems.Any())
            {
                message = $"Member {member.Name} has {expiredItems.Count} expired borrowed items and cannot borrow new items.";
                return false;
            }

            // Check is membership is expired
            if (member.MembershipEndDate < DateTime.Now)
            {
                message = $"Member {member.Name}'s membership has expired on {member.MembershipEndDate:MM/dd/yyyy}  and cannot borrow items.";
                return false;
            }


            libraryItemEntity.IsBorrowed = true;
            _repository.UpdateLibraryItem(libraryItemEntity);

            // Set due date to 3 days from now
            var borrowedDate = DateTime.Now;
            var dueDate = borrowedDate.AddDays(3);

            _repository.AddBorrowedItem(new Domain.Entities.BorrowedItem
            {
                MemberId = memberId,
                LibraryItemId = itemId,
                BorrowedDate = borrowedDate,
                DueDate = dueDate
            });
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

            var borrowedItems = _repository.GetBorrowedItem(memberId, itemId).ToList();
            if (!borrowedItems.Any())
            {
                message = $"'{libraryItemEntity.Title}' was not borrowed by {member.Name}.";
                return false;
            }

            libraryItemEntity.IsBorrowed = false;
            _repository.UpdateLibraryItem(libraryItemEntity);

            foreach (var bi in borrowedItems)
            {
                if (bi.IsActive)
                {
                    bi.IsActive = false;
                    _repository.UpdateBorrowedItem(bi);
                }
            }

            message = $"'{libraryItemEntity.Title}' returned by {member.Name}.";
            return true;
        }

        public IEnumerable<Domain.LibraryItem> GetAllLibraryItems()
        {
            var libraryItemsEntities = _repository.GetAllLibraryItems();
            return libraryItemsEntities.Select(MapToDomainModel);
        }
        private LibraryItem MapToDomainModel(Domain.Entities.LibraryItem entity)
        {
            Domain.LibraryItem model = (LibraryItemTypeEnum)entity.Type switch
            {
                LibraryItemTypeEnum.Book => new Book(entity.Id, entity.Title, entity.Author ?? string.Empty, entity.Pages ?? 0),
                LibraryItemTypeEnum.Magazine => new Magazine(entity.Id, entity.Title, entity.IssueNumber ?? 0, entity.Publisher ?? string.Empty),
                _ => throw new InvalidOperationException("Unknown library item type.")
            };

            // Sync the borrowed state from DB entity to domain model
            if (entity.IsBorrowed)
            {
                model.Borrow();
            }

            return model;
        }
        private Member MapMemberToDomainModel(Domain.Entities.Member entity)
        {
            var member = new Member(entity.Id, entity.Name);

            // Populate borrowed items if any exist
            if (entity.BorrowedItems != null && entity.BorrowedItems.Any())
            {
                foreach (var borrowedItem in entity.BorrowedItems)
                {
                    if (borrowedItem.LibraryItem != null)
                    {
                        Domain.LibraryItem domainItem = (LibraryItemTypeEnum)borrowedItem.LibraryItem.Type switch
                        {
                            LibraryItemTypeEnum.Book => new Book(borrowedItem.LibraryItem.Id,
                                borrowedItem.LibraryItem.Title,
                                borrowedItem.LibraryItem.Author ?? string.Empty,
                                borrowedItem.LibraryItem.Pages ?? 0),
                            LibraryItemTypeEnum.Magazine => new Magazine(borrowedItem.LibraryItem.Id,
                                borrowedItem.LibraryItem.Title,
                                borrowedItem.LibraryItem.IssueNumber ?? 0,
                                borrowedItem.LibraryItem.Publisher ?? string.Empty),
                            _ => throw new InvalidOperationException("Unknown library item type.")
                        };
                        member.BorrowItem(domainItem);
                    }
                }
            }
            return member;
        }

        public Domain.Entities.BorrowedItem? GetBorrowedItem(int memberId, int itemId) =>
        _repository.GetBorrowedItem(memberId, itemId).FirstOrDefault();

        public IEnumerable<Domain.Entities.BorrowedItem> GetMemberBorrowedItems(int memberId, bool activeOnly)
        {
            return activeOnly
                ? _repository.GetActiveBorrowedItemsByMember(memberId)
                : _repository.GetAllBorrowedItemsByMember(memberId);
        }

    }
}