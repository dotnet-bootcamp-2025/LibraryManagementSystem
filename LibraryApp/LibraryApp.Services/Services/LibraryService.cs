
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain;
using LibraryApp.Domain.Enums;
//adaptar el program cs para implementar este servicio

namespace LibraryApp.Application.Services
{
    public sealed class LibraryService:ILibraryService
    {
        private readonly ILibraryAppRepository _repository;

        public LibraryService(ILibraryAppRepository repository)
        {
            _repository = repository;
        }
        public sealed class LibraryService : ILibraryService
        {

            public Book AddBook(string title, string author, int pages = 0)
            {
                //var book = new Book(_nextItemId++, title, author, pages);
                //_items.Add(book);
                //return book;
                throw new NotImplementedException();
            }
            public Magazine AddMagazine(string title, int issueNumber, string publisher)
            {
                //var mag = new Magazine(_nextItemId++, title, issueNumber, publisher);
                //_items.Add(mag);
                //return mag;
                throw new NotImplementedException();
            }
            public Member RegisterMember(string name)
            {
                //var member = new Member(_nextMemberId++, name);
                //_members.Add(member);
                //return member;
                throw new NotImplementedException();
            }
            public IEnumerable<LibraryItem> FindItems(string? term)
            {
                //if (string.IsNullOrWhiteSpace(term)) return _items;
                //term = term.Trim().ToLowerInvariant();
                //return _items.Where(i => i.Title.ToLowerInvariant().Contains(term));
                throw new NotImplementedException();
            }

            //TODO: Is it a GET or a POST?
            public bool BorrowItem(int memberId, int itemId, out string message)
            {
                //var member = _members.FirstOrDefault(m => m.Id == memberId);
                //var item = _items.FirstOrDefault(i => i.Id == itemId);
                //if (member is null) { message = "Member not found."; return false; }
                //if (item is null) { message = "Item not found."; return false; }
                //try
                //{
                //    member.BorrowItem(item);
                //    message = $"'{item.Title}' borrowed by {member.Name}.";
                //    return true;
                //}
                //catch (Exception ex)
                //{
                //    message = ex.Message;
                //    return false;
                //}
                throw new NotImplementedException();
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
            private IEnumerable<Domain.LibraryItem> GetAllLibraryItems()
            {
                var libraryItemsEntities = _repository.GetAllItems();
                return libraryItemsEntities.Select(MapToDomainModel);
            }

            private Domain.LibraryItem MapToDomainModel(Domain.Entities.LibraryItems entity)
            {
                return (LibraryItemTypeEnum)entity.Type switch
                {
                    LibraryItemTypeEnum.Book => new Book(entity.Id, entity.Title, entity.Author ?? "Unknown", entity.Pages ?? 0),
                    LibraryItemTypeEnum.Magazine => new Magazine(entity.Id, entity.Title, entity.IssueNumber ?? 0, entity.Publisher ?? "Unknown"),
                    _ => throw new InvalidOperationException("Unknown library item type.")
                };
            }

        }
    }
}