using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Services;
using Moq;

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

        // A este tipo de test se le conoce como Gherkin, Se centran en la funcionalidad (profundizar)
        [Fact]
        public void WhenABookIsAdded_ThenItShouldBeCreated()
        {
            // Arrange
            var blueDaemonBook = new Domain.Entities.LibraryItem
            {
                Title = "Blue Deamon",
                Author = "John Doe",
                Pages = 50,
                Type = (int)LibraryApp.Domain.Enums.LibraryItemTypeEnum.Book,
                IsBorrowed = false
            };

            _mockRepository
                .Setup(r => r.AddLibraryItem(It.IsAny<Domain.Entities.LibraryItem>()))
                .Callback<Domain.Entities.LibraryItem>(item =>
                {
                    item.Id = 1;
                });

            // Act
            var result = _libraryService.AddBook(
                blueDaemonBook.Title,
                blueDaemonBook.Author,
                blueDaemonBook.Pages.Value
                );

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        // Add Unit test for GetAllLibraryItems
        [Fact]
        public void WhenGetAllLibraryItemsIsCalled_ThenItshouldReturnAllItems()
        {
            // Arrange
            var items = GetItems();
            _mockRepository
                .Setup(r => r.GetAllLibraryItems())
                .Returns(items);

            // Act
            var result = _libraryService.GetAllLibraryItems();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        // Add Unit Test for RegisterMember

        [Fact]
        public void WhenAMemberIsAdded_ThenItShouldBeCreated()
        {
            // Arrange
            var WillDafoeMember = new Domain.Entities.Member
            {
                Name = "Willem Dafoe"
            };

            _mockRepository
                .Setup(r => r.AddMember(It.IsAny<Domain.Entities.Member>()))
                .Callback<Domain.Entities.Member>(member =>
                {
                    member.Id = 1; // DB - ID asignation simulation
                });

            // Act
            var result = _libraryService.RegisterMember(
                WillDafoeMember.Name
                );

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        // Add Unit Test for BorrowItem
        [Fact]
        public void WhenAnItemIsBorrowed_ThenARegisterWithItsIdShouldBeCreated()
        {
            // Arrange
            var WillDafoeMember = new Domain.Entities.Member
            {
                Id = 1,
                Name = "Willem Dafoe",
            };

            var libraryItemEntity = new Domain.Entities.LibraryItem
            {
                Id = 6,
                Title = "Los Muertos del Hambre",
                Author = "Susana Distancia",
                Pages = 123,
                Type = (int)LibraryApp.Domain.Enums.LibraryItemTypeEnum.Book,
                IsBorrowed = false
            };

            //var emptyList = new List<Domain.Entities.BorrowedItem>();

            _mockRepository
                .Setup(r => r.GetMemberById(It.IsAny<int>()))
                .Callback<Domain.Entities.Member>(member =>
                {
                    member.Id = 1; // DB - ID asignation simulation
                });

            _mockRepository
                .Setup(r => r.GetLibraryItem(It.IsAny<int>()))
                .Callback<Domain.Entities.LibraryItem>(item =>
                {
                    item.Id = 6;
                });

            _mockRepository
                .Setup(r => r.GetBorrowedItemsByMember(It.IsAny<int>()))
                .Callback<Domain.Entities.LibraryItem>(item =>
                {
                    item.Id = 6;
                });

            _mockRepository
                .Setup(r => r.AddBorrowedItem(It.IsAny<Domain.Entities.BorrowedItem>()))
                .Callback<List<Domain.Entities.BorrowedItem>>(emptyList =>
                {
                    emptyList = new List<Domain.Entities.BorrowedItem>();
                });

            // Act
            var result = _libraryService.BorrowItem(
                WillDafoeMember.Id,
                libraryItemEntity.Id,
                out string message
                );

            // Assert
            Assert.True(result);
        }

        private static List<Domain.Entities.LibraryItem> GetItems()
        {
            return new List<Domain.Entities.LibraryItem>
            {
                new Domain.Entities.LibraryItem
                {
                    Id = 1,
                    Title = "Blue Deamon",
                    Author = "John Doe",
                    Pages = 50,
                    Type = (int)LibraryApp.Domain.Enums.LibraryItemTypeEnum.Book,
                    IsBorrowed = false
                },
                new Domain.Entities.LibraryItem
                {
                    Id = 2,
                    Title = "Tech Monthly",
                    IssueNumber = 5,
                    Publisher = "Tech Publishers",
                    Type = (int)LibraryApp.Domain.Enums.LibraryItemTypeEnum.Magazine,
                    IsBorrowed = false
                }
            };
        }
    }
}
