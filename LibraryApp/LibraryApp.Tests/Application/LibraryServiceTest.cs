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
        [Fact]
        public void WhenAMemberIsRegistered_ThenItShouldBeRegistered()
        {
            // Arrange
            var leoIracheta = new Domain.Entities.Member
            {
                Name = "Leo Iracheta",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30)
            };

            _mockRepository
                .Setup(r => r.AddMember(It.IsAny<Domain.Entities.Member>()))
                .Callback<Domain.Entities.Member>(item =>
                {
                    item.Id = 10; // Simulate database assigning an ID
                });
            // Act
            var result = _libraryService.RegisterMember(
                leoIracheta.Name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Id);
        }
        [Fact]
        public void WhenAnItemisBorrowed_ThenItShouldBeBorrowed()
        {
            // Arrange
            var memberId = 1;
            var itemId = 5;

            var member = new Domain.Entities.Member
            {
                Id = memberId,
                Name = "Leo Iracheta",
                StartDate = DateTime.UtcNow.AddDays(-10),
                EndDate = DateTime.UtcNow.AddDays(20)
            };
            var libraryItem = new Domain.Entities.LibraryItem
            {
                Id = itemId,
                Title = "Clean Code",
                IsBorrowed = false
            };

            var borrowedItems = new List<Domain.Entities.BorrowedItem>(); 

            _mockRepository.Setup(r => r.GetMemberById(memberId))
                .Returns(member);

            _mockRepository.Setup(r => r.GetLibraryItemById(itemId))
                .Returns(libraryItem);

            _mockRepository.Setup(r => r.GetBorrowedItemsFromMember(memberId))
                .Returns(borrowedItems);

            _mockRepository.Setup(r => r.UpdateLibraryItem(It.IsAny<Domain.Entities.LibraryItem>()));

            _mockRepository.Setup(r => r.AddBorrowedItem(It.IsAny<Domain.Entities.BorrowedItem>()));

            // Act
            var result = _libraryService.BorrowItem(memberId, itemId, out var message);

            // Assert
            Assert.True(result);
            Assert.Contains("borrowed by", message);
            _mockRepository.Verify(r => r.AddBorrowedItem(It.IsAny<Domain.Entities.BorrowedItem>()), Times.Once);
        }
    }
}
