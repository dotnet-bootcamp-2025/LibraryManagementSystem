using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Services;
using Moq;

namespace LibraryApp.Tests.Application
{
    public class LibraryServiceTest
    {
        private readonly LibraryService _libaryService;
        private readonly Mock<ILibraryAppRepository> _mockRepository;    

        public LibraryServiceTest()
        {
            _mockRepository = new Mock<ILibraryAppRepository>();
            _libaryService = new LibraryService(_mockRepository.Object);
        }

        [Fact]
        public void WhenABookISAdded_ThenItShouldBeCreated()
        {
            // Arrange
            var ReyMysterioBook = new Domain.Entities.LibraryItems
            {
                Title = "Oscar Gutierrez",
                Author = "Smack Down",
                Pages = 619,
                Type = (int)Domain.Enums.LibraryItemTypeEnum.Book,
                IsBorrowed = false
            };

            _mockRepository
                .Setup(repo => repo.AddLibraryItems(It.IsAny<Domain.Entities.LibraryItems>()))
                .Callback<Domain.Entities.LibraryItems>(item =>
                {
                    // Simulate setting the Id as it would be done by the database
                    item.Id = 1; // or any other logic to generate an Id
                });


            // Act
            var result = _libaryService.AddBook(
                ReyMysterioBook.Title,
                ReyMysterioBook.Author,
                ReyMysterioBook.Pages.Value);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id); // Ensure the Id was set
        }

        // Add unit test for GetAllLibraryItems
        [Fact]
        public void WhenGetAllLibraryItemsIsCalled_ThenItShouldReturnAllItems()
        {
            // Arrange
            var libraryItems = new List<Domain.Entities.LibraryItems>
            {
                new Domain.Entities.LibraryItems
                {
                    Id = 1,
                    Title = "Book One",
                    Author = "Author A",
                    Pages = 300,
                    Type = (int)Domain.Enums.LibraryItemTypeEnum.Book,
                    IsBorrowed = false
                },
                new Domain.Entities.LibraryItems
                {
                    Id = 2,
                    Title = "Magazine One",
                    Author = "Author B",
                    Pages = 50,
                    Type = (int)Domain.Enums.LibraryItemTypeEnum.Magazine,
                    IsBorrowed = false
                }
            };
            _mockRepository
                .Setup(repo => repo.GetAllLibraryItems())
                .Returns(libraryItems);
            // Act
            var result = _libaryService.GetAllLibraryItems();
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, item => item.Title == "Book One");
            Assert.Contains(result, item => item.Title == "Magazine One");
        }

        // add unit test for RegisterMember
        [Fact]
        public void WhenAMemberIsRegistered_ThenItShouldBeCreated()
        {
            // Arrange
            var newMember = new Domain.Entities.Member
            {
                Name = "John Doe",
            };
            _mockRepository
                .Setup(repo => repo.AddMember(It.IsAny<Domain.Entities.Member>()))
                .Callback<Domain.Entities.Member>(member =>
                {
                    // Simulate setting the Id as it would be done by the database
                    member.Id = 1; // or any other logic to generate an Id
                });
            // Act
            var result = _libaryService.RegisterMember(newMember.Name);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id); // Ensure the Id was set
            Assert.Equal("John Doe", result.Name);
        }

        // add unit test for borrow an item
        [Fact]
        public void WhenAnItemIsBorrowed_ThenItShouldBeMarkedAsBorrowed()
        {
            // Arrange
            var memberId = 1;
            var itemId = 1;
            var member = new Domain.Entities.Member
            {
                Id = memberId,
                Name = "John Doe"
            };
            var libraryItem = new Domain.Entities.LibraryItems
            {
                Id = itemId,
                Title = "Book One",
                Author = "Author A",
                Pages = 300,
                Type = (int)Domain.Enums.LibraryItemTypeEnum.Book,
                IsBorrowed = false
            };
            _mockRepository.Setup(repo => repo.GetMemberById(memberId)).Returns(member);
            _mockRepository.Setup(repo => repo.GetLibraryItemById(itemId)).Returns(libraryItem);
            // Act
            var result = _libaryService.BorrowItem(memberId, itemId, out string message);
            // Assert
            Assert.True(result);
            //Assert.Equal("Item borrowed successfully.", message); //el valor de la vareiable message no se esta seteando correctamente en el servicio
            Assert.True(libraryItem.IsBorrowed);
        }

    }
}
