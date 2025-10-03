using LibraryApp.Application.Abstractions;
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
                Type = (int)LibraryItemTypeEnum.Book,
                author = author,
                Pages = pages,
                IsBorrowed = false
                };

            _repository.AddLibraryItem(bookEntity);

            return new Domain.Book(bookEntity.Id, bookEntity.Title, bookEntity.author);



        }
        public Magazine AddMagazine(string title, int issueNumber, string publisher)
        {
            // 1. Crear la entidad de base de datos (LibraryItem)
            var magazineEntity = new Domain.Entities.LibraryItem
            {
                Title = title,
                Type = (int)LibraryItemTypeEnum.Magazine, // Usamos el Enum para el tipo
                IssueNumber = issueNumber,
                Publisher = publisher,
                IsBorrowed = false
            };

            // 2. Guardar la entidad usando el repositorio
            _repository.AddLibraryItem(magazineEntity);

            // 3. Devolver el modelo de dominio (Magazine)
            // Usamos los datos de la entidad para asegurar que el ID ya está asignado
            return new Domain.Magazine(magazineEntity.Id, magazineEntity.Title, magazineEntity.IssueNumber ?? 0, magazineEntity.Publisher ?? string.Empty);
        }
        public Domain.Member RegisterMember(string name)
        {
            // 1. Crear la entidad de base de datos (Member).
            // Nota: El ID será generado por la base de datos (Identity).
            var memberEntity = new Domain.Entities.Member
            {
                Name = name.Trim(),
            };

            // 2. Guardar la entidad usando el repositorio.            
            _repository.AddMember(memberEntity);           

            // 3. Devolver el modelo de dominio (Domain.Member).
            // El ID de la entidad ya debería haber sido poblado después de la llamada al repositorio.
            return new Domain.Member(memberEntity.Id, memberEntity.Name);
        }
        public IEnumerable<Domain.LibraryItem> FindItems(string? term)
        {
            // 1. Obtener todos los ítems de la base de datos (a través del repositorio)
            var allItems = _repository.GetAlLibraryItems();

            // 2. Si el término de búsqueda es nulo o vacío, devolvemos todos los ítems.
            if (string.IsNullOrWhiteSpace(term))
            {
                // Mapeamos los entities a modelos de dominio antes de devolver
                return allItems.Select(MapToDomainModel);
            }

            // 3. Normalizamos el término de búsqueda.
            term = term.Trim().ToLowerInvariant();

            // 4. Aplicamos el filtro de búsqueda por título.
            var filteredEntities = allItems
                .Where(entity => entity.Title.ToLowerInvariant().Contains(term))
                .ToList(); // Convertimos a lista para ejecutar la consulta

            // 5. Mapeamos los entities filtrados a modelos de dominio antes de devolver.
            return filteredEntities.Select(MapToDomainModel);
        }
               
        public IEnumerable<Domain.Member> Members
        {
            get
            {
                var memberEntities = _repository.GetAllMembers();
                // Mapear las entidades de DB (Entities.Member) a modelos de Dominio (Domain.Member)
                return memberEntities.Select(e => new Domain.Member(e.Id, e.Name));
            }
        }

        //TODO: IS IT A GET OR A POST?
        public bool BorrowItem(int memberId, int itemId, out string message)
        {     
            var memmber = _repository.GetMemberById(memberId);
            if (memmber is null) 
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
                message = $"'{libraryItemEntity.Title}' Item already borrowed.";
                return false;
            }
            libraryItemEntity.IsBorrowed = true;
            _repository.updateLibraryItem(libraryItemEntity);
            _repository.AddBorrowedItem(new Domain.Entities.BorrowedItem {MemberId = memberId, LibraryItemId = itemId});
            message = $"'{libraryItemEntity.Title}' borrowed by {memmber.Name}.";
            return true;

        }
        public bool ReturnItem(int memberId, int itemId, out string message)
        {
            // 1. Verificar si el miembro existe.
            var memberEntity = _repository.GetMemberById(memberId);
            if (memberEntity is null)
            {
                message = "Member not found.";
                return false;
            }

            // 2. Verificar si el artículo existe.
            var libraryItemEntity = _repository.GetLibraryItemById(itemId);
            if (libraryItemEntity is null)
            {
                message = "Item not found.";
                return false;
            }

            // 3. Verificar si el artículo SÍ está prestado.
            if (!libraryItemEntity.IsBorrowed)
            {
                message = $"'{libraryItemEntity.Title}' is not currently borrowed.";
                return false;
            }

            // 4. Verificar si este miembro ES quien lo tiene prestado.
            // Esto requiere un método en el repositorio para encontrar el registro de préstamo.
            var borrowedItem = _repository.GetBorrowedItem(memberId, itemId);

            if (borrowedItem is null)
            {
                // Esto indica que el artículo está marcado como prestado (IsBorrowed=true), 
                // pero no hay un registro de quién lo tiene. Lo tratamos como un error en los datos
                // o que otro miembro lo tiene, pero el servicio lo maneja como 'no encontrado para este miembro'.
                message = $"Error: Item is borrowed, but not by member {memberEntity.Name}.";
                return false;
            }

            // 5. Actualizar el estado del artículo y eliminar el registro de préstamo.
            libraryItemEntity.IsBorrowed = false;

            _repository.UpdateLibraryItem(libraryItemEntity); // Actualiza la entidad en DB (IsBorrowed = false)
            _repository.RemoveBorrowedItem(borrowedItem);     // Elimina el registro de la tabla BorrowedItems

            message = $"'{libraryItemEntity.Title}' returned by {memberEntity.Name}.";
            return true;
        }

        public IEnumerable<Domain.LibraryItem> GetAllLibraryItems()
        {
            var LibraryItemsEntities = _repository.GetAlLibraryItems();

            return LibraryItemsEntities.Select(MapToDomainModel);
        }

        private Domain.LibraryItem MapToDomainModel(Domain.Entities.LibraryItem entity)
        {
            return (LibraryItemTypeEnum)entity.Type switch
            {
                LibraryItemTypeEnum.Book => new Book(entity.Id, entity.Title, entity.author ?? string.Empty, entity.Pages ?? 0),
                LibraryItemTypeEnum.Magazine => new Magazine(entity.Id, entity.Title, entity.IssueNumber ?? 0, entity.Publisher ?? string.Empty),
                _ => throw new InvalidOperationException("Unknown library item type.")
            };
        }
    }
}
