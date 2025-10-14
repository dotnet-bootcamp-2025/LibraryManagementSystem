//using LibraryApp.Application.Abstractions;
//using LibraryApp.Application.Services;
//using Moq;

//namespace LibraryApp.Tests.Application
//{
//    public class LibraryServiceTest
//    {
//        private readonly LibraryService _libraryService;
//        private readonly Mock<ILibraryAppRepository> _mockRepository;

//        public LibraryServiceTest()
//        {
//            _mockRepository = new Mock<ILibraryAppRepository>();
//            _libraryService = new LibraryService(_mockRepository.Object);
//        }

//        [Fact]
//        public void WhenABookIsAdded_ThenItshouldBeCreated()
//        {
//            // Arrange
//            var blueDemonBook = new Domain.Entities.LibraryItem
//            {
//                Title = "Blue Demon",
//                Author = "John Doe",
//                Pages = 50,
//                Type = (int)LibraryApp.Domain.Enums.LibraryItemTypeEnum.Book,
//                IsBorrowed = false
//            };

//            _mockRepository
//                .Setup(r => r.AddLibraryItem(It.IsAny<Domain.Entities.LibraryItem>()))
//                .Callback<Domain.Entities.LibraryItem>(item =>
//                {
//                    item.Id = 1; // Simulate database assigning an ID
//                });

//            // Act
//            var result = _libraryService.AddBook(
//                blueDemonBook.Title,
//                blueDemonBook.Author,
//                blueDemonBook.Pages.Value);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(1, result.Id);
//        }

//        // Add Unit test for GetAllLibraryItems
//        [Fact]
//        public void WhenGetAllLibraryItemsIsCalled_ThenItshouldReturnAllItems()
//        {
//            // Arrange
//            var items = GetItems();
//            _mockRepository
//                .Setup(r => r.GetAllLibraryItems())
//                .Returns(items);

//            // Act
//            var result = _libraryService.GetAllLibraryItems();

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(2, result.Count());
//        }


//        [Fact]
//        public void WhenAMemberIsRegistered_ThenItShouldBeCreated()
//        {
//            // Arrange
//            var newMember = new Domain.Entities.Member
//            {
//                Name = "Silvia Cruz",
                
//            };

//            _mockRepository
//                .Setup(r => r.RegisterMember(It.IsAny<Domain.Entities.Member>()))
//                .Callback<Domain.Entities.Member>(member => member.Id = 1);

//            // Act
//            var result = _libraryService.RegisterMember(newMember.Name);
            

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(1, result.Id);
//            Assert.Equal("Silvia Cruz", result.Name);
            

//            // Verify that the repository method was called exactly once
//            _mockRepository.Verify(r => r.RegisterMember(It.IsAny<Domain.Entities.Member>()), Times.Once);
//        }


//        private static List<Domain.Entities.LibraryItem> GetItems()
//        {
//            return new List<Domain.Entities.LibraryItem>
//            {
//                new Domain.Entities.LibraryItem
//                {
//                    Id = 1,
//                    Title = "Blue Demon",
//                    Author = "John Doe",
//                    Pages = 50,
//                    Type = (int)LibraryApp.Domain.Enums.LibraryItemTypeEnum.Book,
//                    IsBorrowed = false
//                },
//                new Domain.Entities.LibraryItem
//                {
//                    Id = 2,
//                    Title = "Tech Monthly",
//                    IssueNumber = 5,
//                    Publisher = "Tech Publishers",
//                    Type = (int)LibraryApp.Domain.Enums.LibraryItemTypeEnum.Magazine,
//                    IsBorrowed = false
//                }
//            };
//        }
//    }
//}
