using LibraryApp.Application.Abstractions;
using LibraryApp.Domain;
using LibraryApp.Domain.Entities;
using LibraryApp.Domain.Enums;
using LibraryApp.Application.Services;

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
            var allItems = _repository.GetAllLibraryItems();

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
            //var dbPath = _repository.GetDatabasePath();
            //Console.WriteLine($"🔥🔥🔥 LA BASE DE DATOS ESTÁ AQUÍ: {dbPath} 🔥🔥🔥");

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

            int activeCount = _repository.GetActiveBorrowedItemCountByMemberId(memberId);

            if (activeCount >= 3)
            {
                message = $"{member.Name} cannot borrow more items. They currently have {activeCount} items borrowed (Limit is 3).";
                return false;
            }

            if (libraryItemEntity.IsBorrowed)
            {
                var existingBorrowedItem = _repository.GetBorrowedItemByItemId(itemId);

                if (existingBorrowedItem != null)
                {
                    // ============ CÓDIGO DE DEPURACIÓN - INICIO ============
                    Console.WriteLine($"=== DEBUG INFO ===");
                    Console.WriteLine($"Item ID: {itemId}");
                    Console.WriteLine($"BorrowDate from DB (raw): {existingBorrowedItem.BorrowDate}");
                    Console.WriteLine($"BorrowDate.Kind: {existingBorrowedItem.BorrowDate.Kind}");
                    Console.WriteLine($"BorrowDate.ToString('o'): {existingBorrowedItem.BorrowDate.ToString("o")}");
                    Console.WriteLine($"DateTime.UtcNow: {DateTime.UtcNow}");
                    Console.WriteLine($"DateTime.UtcNow.Date: {DateTime.UtcNow.Date}");

                    TimeSpan duration = DateTime.UtcNow.Date - existingBorrowedItem.BorrowDate;
                    Console.WriteLine($"Duration: {duration}");
                    Console.WriteLine($"Duration.TotalDays: {duration.TotalDays}");
                    Console.WriteLine($"Duration.TotalHours: {duration.TotalHours}");
                    Console.WriteLine($"Is Expired (>= 3 days)?: {duration.TotalDays >= 3.0}");
                    Console.WriteLine($"=== END DEBUG ===");
                    // ============ CÓDIGO DE DEPURACIÓN - FIN ============

                    message = $"Item '{libraryItemEntity.Title}' is already borrowed. Borrowed Date: {existingBorrowedItem.BorrowDate}.";
                    return false;
                    // Si han pasado 3 días o más, está vencido
                    if (duration.TotalDays >= 3.0)
                    {
                        message = "Expired borrow item. The currently borrowed item is overdue and cannot be loaned again.";
                        return false;
                    }
                }

                message = $"Item '{libraryItemEntity.Title}' is marked as borrowed, but no active loan was found.";
                return false;

                // 🛑 Si no está vencido, o si no se encontró activo, sale con este mensaje.
                message = $"'{libraryItemEntity.Title}' Item already borrowed.";
                return false;
            }

            try
            {
                var borrowedItem = new BorrowedItem
                {
                    LibraryItemId = itemId,
                    MemberId = memberId,
                    BorrowDate = DateTime.Now,
                    IsActive = true
                };


                libraryItemEntity.IsBorrowed = true;
            _repository.updateLibraryItem(libraryItemEntity);
                        
            _repository.AddBorrowedItem(new Domain.Entities.BorrowedItem
            {
                MemberId = memberId,
                LibraryItemId = itemId,
                BorrowDate = DateTime.UtcNow.Date // <-- Registro de la fecha actual
            });

            message = $"'{libraryItemEntity.Title}' borrowed by {member.Name}.";
            return true;

        }
            catch (Exception ex)
            {
                message = $"An error occurred during borrowing: {ex.Message}";
                return false;
            }
        }

        public BorrowItemResponse BorrowItem(int memberId, int itemId)
        {
            string message = "";

            var member = _repository.GetMemberById(memberId);
            if (member == null)
            {
                message = "Member not found.";
                return new BorrowItemResponse(false, message);
            }

            // --- LÓGICA DE VALIDACIÓN DE MEMBRESÍA ---
            if (member.ExpirationDate < DateTime.Now)
            {
                message = $"Membership for {member.Name} expired on {member.ExpirationDate.ToShortDateString()}. Cannot borrow items.";
                return new BorrowItemResponse(false, message);
            }
            // ----------------------------------------

            var libraryItemEntity = _repository.GetLibraryItemById(itemId);
            if (libraryItemEntity == null)
            {
                message = "Item not found.";
                return new BorrowItemResponse(false, message);
            }

            int activeCount = _repository.GetActiveBorrowedItemCountByMemberId(memberId);

            if (activeCount >= 3)
            {
                message = $"{member.Name} cannot borrow more items. They currently have {activeCount} items borrowed (max {3}).";
                return new BorrowItemResponse(false, message);
            }

            if (libraryItemEntity.IsBorrowed)
            {
                // Si está prestado, puedes obtener la info si es necesario, pero aquí solo se informa
                message = $"Item '{libraryItemEntity.Title}' is already borrowed.";
                return new BorrowItemResponse(false, message);
            }

            // Realizar el préstamo
            var borrowedItem = new BorrowedItem
            {
                LibraryItemId = itemId,
                MemberId = memberId,
                BorrowDate = DateTime.Now,
                IsActive = true
            };
            _repository.AddBorrowedItem(borrowedItem);

            // Actualizar el estado del ítem de la biblioteca
            libraryItemEntity.IsBorrowed = true;
            _repository.UpdateLibraryItem(libraryItemEntity);

            message = $"Item '{libraryItemEntity.Title}' successfully borrowed by {member.Name}.";
            return new BorrowItemResponse(true, message);
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

            
            var borrowedItem = _repository.GetBorrowedItemByItemId(itemId); 

            if (borrowedItem is null)
            {
                // El ítem de librería está marcado como prestado (IsBorrowed=true), pero no hay un registro
                // activo en BorrowedItems. Esto es una inconsistencia que debemos tratar como error.
                message = $"Error: Item is marked as borrowed, but no active loan record was found.";
                return false;                
            }

            libraryItemEntity.IsBorrowed = false;

            _repository.UpdateLibraryItem(libraryItemEntity); 
            _repository.DeactivateBorrowedItem(itemId);     

            message = $"'{libraryItemEntity.Title}' returned by {memberEntity.Name}.";
            return true;
        }

        public bool ReturnItem(int itemId, out string message)
        {
            var libraryItemEntity = _repository.GetLibraryItemById(itemId);
            if (libraryItemEntity is null)
            {
                message = "Item not found in the library.";
                return false;
            }

            if (!libraryItemEntity.IsBorrowed)
            {
                message = $"'{libraryItemEntity.Title}' is not currently borrowed.";
                return false;
            }

            _repository.DeactivateBorrowedItem(itemId);

            libraryItemEntity.IsBorrowed = false;
            _repository.updateLibraryItem(libraryItemEntity); 

            message = $"'{libraryItemEntity.Title}' returned successfully.";
            return true;
        }

        //public IEnumerable<Domain.LibraryItem> GetAllLibraryItems()
        //{
        //    var LibraryItemsEntities = _repository.GetAlLibraryItems();

        //    return LibraryItemsEntities.Select(MapToDomainModel);
        //}

        private Domain.LibraryItem MapToDomainModel(Domain.Entities.LibraryItem entity)
        {
            return (LibraryItemTypeEnum)entity.Type switch
            {
                LibraryItemTypeEnum.Book => new Book(entity.Id, entity.Title, entity.author ?? string.Empty, entity.Pages ?? 0),
                LibraryItemTypeEnum.Magazine => new Magazine(entity.Id, entity.Title, entity.IssueNumber ?? 0, entity.Publisher ?? string.Empty),
                _ => throw new InvalidOperationException("Unknown library item type.")
            };
        }

        IEnumerable<Domain.Entities.LibraryItem> ILibraryService.FindItems(string? term)
        {
            throw new NotImplementedException();
        }

        Domain.Entities.Member ILibraryService.RegisterMember(string name)
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddYears(1); // Membresía válida por 1 año

            var newMember = new Domain.Entities.Member
            {
                Name = name,
                StartDate = startDate,
                ExpirationDate = endDate // La fecha de expiración se establece en 1 año
            };

            _repository.AddMember(newMember);

            // La firma requiere devolver la entidad del dominio
            return newMember;
        }

        public IEnumerable<Domain.LibraryItem> GetAllLibraryItems()
        {
            return _repository.GetAllLibraryItems()
                .Select(entity => MapToDomainModel(entity))
                .ToList();
        }

        public List<BorrowedItem> GetBorrowedItemsByMemberId(int memberId)
        {
            return _repository.GetBorrowedItemsByMemberId(memberId);
        }

        IEnumerable<Domain.Entities.LibraryItem> ILibraryService.GetAllLibraryItems()
        {
            return _repository.GetAllLibraryItems();
        }
    }
}
