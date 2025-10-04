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
            var memberEntity = new Domain.Entities.Member
            {
                Name = name
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
            //if borrowed item is already in table, remove it first to prevent duplicates (refuerzo)

            var borrowedItems = _repository.GetBorrowedItem(memberId, itemId).ToList();
            foreach (var bi in borrowedItems)
            {
                _repository.RemoveBorrowedItem(bi);
            }

            libraryItemEntity.IsBorrowed = true;
            _repository.UpdateLibraryItem(libraryItemEntity);
            _repository.AddBorrowedItem(new Domain.Entities.BorrowedItem { MemberId = memberId, LibraryItemId = itemId });
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

            //(refuerzo)cleanup historical duplicates if any to prevent overpopulation of BorrowedItems table to have the current state of borrowed items only
            foreach (var bi in borrowedItems)
            {
                _repository.RemoveBorrowedItem(bi);
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

        public Domain.Entities.BorrowedItem? RemoveBorrowedItem(int memberId, int itemId)
        {
            var borrowed = _repository.GetBorrowedItem(memberId, itemId).FirstOrDefault();
            if (borrowed != null) _repository.RemoveBorrowedItem(borrowed);
            return borrowed;
        }
    }
 }