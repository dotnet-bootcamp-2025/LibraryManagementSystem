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
            //var mag = new Magazine(_nextItemId++, title, issueNumber, publisher);
            //_items.Add(mag);
            //return mag;
            throw new NotImplementedException();
        }
        public Domain.Member RegisterMember(string name)
        {
            //var member = new Member(_nextMemberId++, name);
            //_members.Add(member);
            //return member;
            throw new NotImplementedException();
        }
        public IEnumerable<Domain.LibraryItem> FindItems(string? term)
        {
            //if (string.IsNullOrWhiteSpace(term)) return _items;
            //term = term.Trim().ToLowerInvariant();
            //return _items.Where(i => i.Title.ToLowerInvariant().Contains(term));
            throw new NotImplementedException();
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
    }
}
