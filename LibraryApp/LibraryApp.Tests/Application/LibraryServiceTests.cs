using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Tests.Application
{
    public class LibraryServiceTest
    {
        private readonly LibraryService _libraryService;
        private readonly Mock<ILibraryAppRepository> _mockRepository;

        public LibraryServiceTest()
        {
            _mockRepository = new Mock<ILibraryAppRepository>();
            _libraryService = new LibraryService(_mockRepository.Object);
        }

        [Fact]
        public void WhenBookIsAdded_ThenItshouldBeCreated()
        {
            // Arrange
            var blueDemonBook = new Domain.Entities.LibraryItem
            {
                Title = "Blue Demon",
                Type = (int)LibraryApp.Domain.Enums.LibraryItemTypeEnum.Book,
                author = "Blue Demon",
                Pages = 300,
                IsBorrowed = false
            };

            _mockRepository
                .Setup(repo => repo.AddLibraryItem(It.IsAny<Domain.Entities.LibraryItem>()))
                .Callback<Domain.Entities.LibraryItem>(item =>
                {
                    // Simular la asignación de ID como lo haría la base de datos
                    item.Id = 1; // Asignar un ID simulado
                });

            // Act
            var result = _libraryService.AddBook(
                blueDemonBook.Title,
                blueDemonBook.author,
                blueDemonBook.Pages.Value);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id); // Verificar que el ID fue asignado

        }

        //add unit test for GetAllLibraryItems
        [Fact]
        public void WhenGetAllLibraryItems_ThenItshouldReturnAllItems()
        {
            List<Domain.Entities.LibraryItem> items = GetItems();

            _mockRepository
                .Setup(r => r.GetAllLibraryItems())
                .Returns(items);

            // Act
            var result = _libraryService.GetAllLibraryItems();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        private static List<Domain.Entities.LibraryItem> GetItems()
        {
            // Arrange
            return new List<Domain.Entities.LibraryItem>
            {
                new Domain.Entities.LibraryItem
                {
                    Id = 1,
                    Title = "Titlex",
                    author = "Authorx",
                    Pages = 50,
                    IsBorrowed = false,
                    Type = (int)LibraryApp.Domain.Enums.LibraryItemTypeEnum.Book
                },
                new Domain.Entities.LibraryItem
                {
                    Id = 2,
                    Title = "Titlexx",
                    IssueNumber = 5,
                    Publisher = "Tech Pub",
                    Type = (int)LibraryApp.Domain.Enums.LibraryItemTypeEnum.Magazine,
                    IsBorrowed = false
                }
            };
        }
    }
}
