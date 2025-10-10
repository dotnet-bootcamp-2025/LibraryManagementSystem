using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        public void WhenABookIsAdded_ThenItShouldBeCreated()
        {
            // Arrange
            var blueDeamonBook = new Domain.Entities.LibraryItem
            {
                Title = "The Blue Deamon",
                Author = "John Smith",
                Pages = 300,
                Type = (int)LibraryApp.Domain.Enum.LibraryItemTypeEnum.Book,
                IsBorrowed = false
            };

            _mockRepository
                .Setup(r => r.AddLibraryItem(It.IsAny<Domain.Entities.LibraryItem>()))
                .Callback<Domain.Entities.LibraryItem>(item => 
                {
                    item.Id = 1; // Simulate database assigning an ID
                });

            // Act
            var result = _libraryService.AddBook(
                blueDeamonBook.Title, 
                blueDeamonBook.Author, 
                (int)blueDeamonBook.Pages);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);

        }
        public void WhenABookIsAdded_ThenItShouldThrowAnException()
        {

        }
    }
}
