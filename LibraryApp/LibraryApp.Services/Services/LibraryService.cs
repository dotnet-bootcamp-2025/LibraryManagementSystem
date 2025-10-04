using LibraryApp.Application.Abstractions;
using LibraryApp.Domain;
using LibraryApp.Domain.Entities;
using LibraryApp.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

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
            return new Domain.Magazine(magazineEntity.Id, magazineEntity.Title, magazineEntity.IssueNumber ?? 0, magazineEntity.Publisher!);
        }
        public Domain.Member RegisterMember(string name)
        {
            var memberEntity = new Domain.Entities.Member
            {
                Name = name
            };

            _repository.AddMember(memberEntity);
            return new Domain.Member(memberEntity.Id, memberEntity.Name);
        }
        public IEnumerable<Domain.LibraryItem> FindItems(string? bookname)
        {
            Console.WriteLine("FindItems called with the bookname: " + bookname);
            if (string.IsNullOrWhiteSpace(bookname)) 
            {
                var allItems = _repository.GetAllLibraryItems();
                Console.WriteLine($"Returning all items, count: {allItems.Count()}");
                return allItems.Select(MapToDomainModel);
            }
            var filteredItems = _repository.GetAllLibraryItems()
                .Where(item => item.Title.Contains(bookname, StringComparison.OrdinalIgnoreCase) ||
                               (item.Author != null && item.Author.Contains(bookname, StringComparison.OrdinalIgnoreCase)) ||
                               (item.Publisher != null && item.Publisher.Contains(bookname, StringComparison.OrdinalIgnoreCase)))
                .ToList();
            return filteredItems.Select(MapToDomainModel);
        }
        // TODO: Is it a GET or a POST?
        public bool BorrowItem(int memberId, int itemId, out string message)
        {
            //var member = _members.FirstOrDefault(m => m.Id == memberId);
            //var item = _items.FirstOrDefault(i => i.Id == itemId);

            var memberEntity = _repository.GetMemberById(memberId);

            if (memberEntity is null) 
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
            
            libraryItemEntity.IsBorrowed = true;

            _repository.UpdateLibraryItem(libraryItemEntity);
            _repository.AddBorrowedItem(new Domain.Entities.BorrowedItem { MemberId = memberId, LibraryItemId = itemId });

            message = $"'{libraryItemEntity.Title}' borrowed by {memberEntity.Name}.";
            return true;
        }
        public bool ReturnItem(int memberId, int itemId, out string message)
        {
            var memberEntity = _repository.GetMemberById(memberId);

            if (memberEntity is null)
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
            _repository.AddBorrowedItem(new Domain.Entities.BorrowedItem { MemberId = memberId, LibraryItemId = itemId });

            message = $"'{libraryItemEntity.Title}' returned by {memberEntity.Name}.";
            return true;
        }

        public IEnumerable<Domain.LibraryItem> GetAllLibraryItems()
        {
            var libraryItemsEntities = _repository.GetAllLibraryItems();
            return libraryItemsEntities.Select(MapToDomainModel);
        }

        private Domain.LibraryItem MapToDomainModel(Domain.Entities.LibraryItem entity)
        {
            return (LibraryItemTypeEnum)entity.Type switch
            {
                LibraryItemTypeEnum.Book => new Book(entity.Id, entity.Title, entity.Author!, entity.Pages ?? 0),
                LibraryItemTypeEnum.Magazine => new Magazine(entity.Id, entity.Title, entity.IssueNumber ?? 0, entity.Publisher!),
                _ => throw new InvalidOperationException("Unknown library item type.")
            };
        }

        public IEnumerable<Domain.Member> GetAllMembers()
        {
            var memberEntities = _repository.GetAllMembers();
            return memberEntities.Select(MapToDomainModelMember);
        }

        private Domain.Member MapToDomainModelMember(Domain.Entities.Member entity2)
        {
            return new Domain.Member(entity2.Id, entity2.Name);
        }

    }
}
