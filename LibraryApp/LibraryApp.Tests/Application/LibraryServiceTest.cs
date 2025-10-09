using LibraryApp.Application;
using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Services;
using LibraryApp.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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
        public void WhenAddBookIsAdded_ThenItShouldBeCreated()
        {
            // Arrange - setear codigo necesario para el test
            var blueDaemonBook = new Domain.Entities.LibraryItem
            {
                Title = "The Blue Daemon",
                Author = "John Smith",
                Pages = 300,
                Type = (int)LibraryApp.Domain.Entities.Enums.LibraryItemTypeEnum.Book,
                IsBorrowed = false
            };
            _mockRepository.Setup(r => r.AddLibraryItem(It.IsAny<Domain.Entities.LibraryItem>()))
                .Callback<Domain.Entities.LibraryItem>(item => item.Id = 1); // Simulate setting the ID when adding

            // Act
            var result = _libraryService.AddBook(
                blueDaemonBook.Title,
                blueDaemonBook.Author,
                blueDaemonBook.Pages);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);

            //Add Unit test for GetAllLibraryItems            
        }
        [Fact]
        public void WhenFindItemsIsCalledWithNull_ThenItShouldReturnAllItems()
        {
            // Arrange
            var items = GetItems();
            _mockRepository.Setup(r => r.GetAllLibraryItems()).Returns(items);
            // Act
            var result = _libraryService.GetAllLibraryItems();
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        private static List<Domain.Entities.LibraryItem> GetItems()
        {
            return new List<Domain.Entities.LibraryItem>
            {
                new Domain.Entities.LibraryItem
                {
                    Id = 1,
                    Title = "The Blue Daemon",
                    Author = "John Smith",
                    Pages = 300,
                    Type = (int)LibraryApp.Domain.Entities.Enums.LibraryItemTypeEnum.Book,
                    IsBorrowed = false
                },
                new Domain.Entities.LibraryItem
                {
                    Id = 2,
                    Title = "Tech Monthly",
                    IssueNumber = 42,
                    Publisher = "Tech Publishers",
                    Type = (int)LibraryApp.Domain.Entities.Enums.LibraryItemTypeEnum.Magazine,
                    IsBorrowed = false
                }
            };
        }
    }
}
