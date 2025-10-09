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

        public void WhenABookIsAdded_ThenItShouldThrowAnException()
        {
            // Arrange
            // Act
            // Assert
        }

    }
}
