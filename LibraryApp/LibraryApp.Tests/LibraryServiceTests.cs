using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Services;
using LibraryApp.Domain.Entities;
using Moq;
using System;
using Xunit;
using System.Collections.Generic;

// Importante: Asegúrate de añadir referencias a los proyectos Application y Domain en tu proyecto de Tests.

namespace LibraryApp.Tests
{
    public class LibraryServiceTests
    {
        private readonly Mock<ILibraryAppRepository> _mockRepository;
        private readonly LibraryService _service;

        public LibraryServiceTests()
        {
            // Inicialización de la configuración para cada prueba
            _mockRepository = new Mock<ILibraryAppRepository>();
            _service = new LibraryService(_mockRepository.Object);
        }

        // --- 1. UNIT TEST PARA REGISTRAR UN MIEMBRO ---

        [Fact]
        public void RegisterMember_ShouldAddMemberWithCorrectDates()
        {
            // Arrange
            string memberName = "Alice Smith";
            // Usaremos una captura para verificar el objeto Member que se le pasa al repositorio
            Member capturedMember = null;

            // Configurar el mock para que capture el objeto 'Member' que se le pasa al método AddMember
            // La implementación explícita ILibraryService.RegisterMember llama internamente a _repository.AddMember(newMember);
            _mockRepository.Setup(r => r.AddMember(It.IsAny<Member>()))
                           .Callback<Member>(m => capturedMember = m);

            // Act
            // La interfaz explícita se llama a través del casting
            var registeredMember = ((ILibraryService)_service).RegisterMember(memberName);

            // Assert
            // 1. Verificar que el repositorio fue llamado para añadir al miembro
            _mockRepository.Verify(r => r.AddMember(It.IsAny<Member>()), Times.Once);

            // 2. Verificar que el miembro devuelto tenga el nombre y se haya capturado correctamente
            Assert.NotNull(capturedMember);
            Assert.Equal(memberName, capturedMember.Name);
            Assert.Equal(registeredMember.Name, memberName);

            // 3. Verificar la lógica de fechas (asumiendo membresía de 1 año)
            Assert.True(capturedMember.StartDate.Date == DateTime.Now.Date);
            // La fecha de finalización debe ser aproximadamente 1 año después
            Assert.True(capturedMember.ExpirationDate.Date == DateTime.Now.AddYears(1).Date);
        }

        // --- 2. UNIT TESTS PARA PEDIR PRESTADO UN ÍTEM (BORROW) ---

        [Fact]
        public void BorrowItem_WhenMembershipIsExpired_ShouldReturnFalse()
        {
            // Arrange
            string message;
            int memberId = 1;
            int itemId = 101;

            // Simular que el ítem EXISTE para que el servicio pase la validación inicial del ítem.
            var existingItem = new LibraryItem { Id = itemId, Title = "Test Item", IsBorrowed = false };

            // *******************************************************************
            // CORRECCIÓN: Usamos una fecha fija en el pasado para evitar problemas de precisión con DateTime.Now
            var expiredMember = new Member
            {
                Id = memberId,
                Name = "Expired Member",
                ExpirationDate = new DateTime(2023, 1, 1) // Fecha fija de expiración en el pasado
            };
            // *******************************************************************

            _mockRepository.Setup(r => r.GetMemberById(memberId))
                           .Returns(expiredMember);

            // Configurar el mock para que el ítem sea encontrado
            _mockRepository.Setup(r => r.GetLibraryItemById(itemId))
                           .Returns(existingItem);


            // Act
            bool result = _service.BorrowItem(memberId, itemId, out message);

            // Assert
            Assert.False(result);
            Assert.Contains("expired", message);
            // Verificar que no se intentó realizar el préstamo ni actualizar el ítem
            _mockRepository.Verify(r => r.AddBorrowedItem(It.IsAny<BorrowedItem>()), Times.Never);
        }

        [Fact]
        public void BorrowItem_WhenMaxItemsReached_ShouldReturnFalse()
        {
            // Arrange
            string message;
            int memberId = 2;
            int itemId = 102;

            var activeMember = new Member { Id = memberId, Name = "Active Member", ExpirationDate = DateTime.Now.AddYears(1) };
            var libraryItem = new LibraryItem { Id = itemId, Title = "Test Item", IsBorrowed = false };

            _mockRepository.Setup(r => r.GetMemberById(memberId)).Returns(activeMember);
            _mockRepository.Setup(r => r.GetLibraryItemById(itemId)).Returns(libraryItem);

            // Simular que el miembro ya tiene 3 ítems prestados (el máximo)
            _mockRepository.Setup(r => r.GetActiveBorrowedItemCountByMemberId(memberId)).Returns(3);

            // Act
            bool result = _service.BorrowItem(memberId, itemId, out message);

            // Assert
            Assert.False(result);
            Assert.Contains("cannot borrow more items", message);
            _mockRepository.Verify(r => r.AddBorrowedItem(It.IsAny<BorrowedItem>()), Times.Never);
        }

        [Fact]
        public void BorrowItem_WhenSuccessful_ShouldReturnTrueAndUpdateStatus()
        {
            // Arrange
            string message;
            int memberId = 3;
            int itemId = 103;

            // Simular que el miembro tiene 2 ítems prestados (Max 3)
            const int currentBorrowedCount = 2;

            var activeMember = new Member { Id = memberId, Name = "Success Member", ExpirationDate = DateTime.Now.AddYears(1) };
            var libraryItem = new LibraryItem { Id = itemId, Title = "Available Book", IsBorrowed = false };

            _mockRepository.Setup(r => r.GetMemberById(memberId)).Returns(activeMember);
            _mockRepository.Setup(r => r.GetLibraryItemById(itemId)).Returns(libraryItem);
            _mockRepository.Setup(r => r.GetActiveBorrowedItemCountByMemberId(memberId)).Returns(currentBorrowedCount);

            // Variable para capturar el ítem actualizado
            LibraryItem updatedItem = null;
            _mockRepository.Setup(r => r.UpdateLibraryItem(It.IsAny<LibraryItem>()))
                           .Callback<LibraryItem>(item => updatedItem = item);

            // Act
            bool result = _service.BorrowItem(memberId, itemId, out message);

            // Assert
            // 1. Préstamo Exitoso
            Assert.True(result);
            // CORRECCIÓN: Buscar una sub-cadena que coincida con el formato real del mensaje de éxito.
            Assert.Contains("borrowed by", message);

            // 2. Verificar que se añadieron registros de préstamo
            _mockRepository.Verify(r => r.AddBorrowedItem(It.IsAny<BorrowedItem>()), Times.Once);

            // 3. Verificar que el estado del ítem fue actualizado a prestado (IsBorrowed = true)
            _mockRepository.Verify(r => r.UpdateLibraryItem(It.IsAny<LibraryItem>()), Times.Once);
            Assert.True(updatedItem.IsBorrowed);
        }
    }
}
