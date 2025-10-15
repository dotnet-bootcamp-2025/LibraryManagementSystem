using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Services;
using LibraryApp.Domain.Entities;
using Moq;

namespace LibraryApp.Tests.Application
{
    public class LibraryServiceTest2
    {
        private readonly LibraryService _libraryService;
        private readonly Mock<ILibraryAppRepository> _mockRepository;

        public LibraryServiceTest2()
        {
            _mockRepository = new Mock<ILibraryAppRepository>();
            _libraryService = new LibraryService(_mockRepository.Object);
        }

        #region GET DATA TESTS

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

        #endregion GET DATA TESTS

        #region ADD DATA TESTS

        [Fact]
        public void WhenAMemberIsRegistered_ItShouldCreateIt()
        {
            //Arrange
            var uncleBobMember = GetMockMember();

            _mockRepository.
                Setup(r => r.AddMember(It.IsAny<Domain.Entities.Member>()))
                .Callback<Domain.Entities.Member>(m =>
                {
                    m.Id = 1;
                });

            //Act
            var result = _libraryService.RegisterMember(uncleBobMember.Name);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void WhenABookIsAdded_ThenItshouldBeCreated()
        {
            // Arrange
            var blueDeamonBook = new Domain.Entities.LibraryItem
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
                blueDeamonBook.Title,
                blueDeamonBook.Author,
                blueDeamonBook.Pages.Value);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        #endregion ADD DATA TESTS

        #region BORROW TESTS

        [Fact]
        public void WhenAMemberBorrowsAnItem_ItShouldBorrowIt()
        {
            //Arrange
            var member = GetMockMember();

            var item = GetMockItem();

            var borrowedRecordMock = new Domain.Entities.BorrowedItem
            {
                Id = 1,
                BorrowedDate = DateOnly.FromDateTime(DateTime.Today)
            };

            _mockRepository.Setup((r => r.GetMemberById(It.IsAny<int>())))
                .Returns(member);

            _mockRepository.Setup((r => r.GetItemById(It.IsAny<int>())))
                .Returns(item);

            _mockRepository.Setup((r => r.AddBorrowedItem(It.IsAny<BorrowedItem>())))
                .Callback<BorrowedItem>(b =>
                {
                    b.Id = 1;
                });

            //Act
            var result = _libraryService.BorrowItem(1, 1, out var message);

            //Assert
            Assert.True(result);
            Assert.Equal($"Item borrowed successfully by {member.Name}.\n Record Id= {borrowedRecordMock.Id}", message);
        }

        [Fact]
        public void WhenAMemberWithSeveralBorrowedItemsBorrowsAnItem_ItShouldReturnFalse()
        {
            //Arrange
            var member = new Domain.Entities.Member
            {
                Id = 1,
                Name = "Uncle Bob",
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                EndDate = DateOnly.FromDateTime(DateTime.Today).AddDays(7)
            };

            var item = new Domain.Entities.LibraryItem
            {
                Id = 1,
                Title = "Pragmatic",
                Author = "Uncle Bob",
                Pages = 50,
                Type = (int)Domain.Enums.LibraryItemTypeEnum.Book,
                IsBorrowed = false
            };

            var mockItems = GetMockBorrowedItems();

            _mockRepository.Setup((r => r.GetMemberById(It.IsAny<int>())))
                .Returns(member);

            _mockRepository.Setup((r => r.GetItemById(It.IsAny<int>())))
                .Returns(item);

            _mockRepository.Setup((r => r.GetMemberBorrowedItems(It.IsAny<int>())))
                .Returns(mockItems);

            //Act
            var result = _libraryService.BorrowItem(1, 1, out var message);

            //Assert
            Assert.False(result);
            Assert.Equal("Members can not borrow more than 3 items.", message);
        }

        [Fact]
        public void WhenAMemberWithOverdueItemsBorrowsAnItem_ItShouldReturnFalse()
        {
            //Arrange
            var member = new Domain.Entities.Member
            {
                Id = 1,
                Name = "Uncle Bob",
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                EndDate = DateOnly.FromDateTime(DateTime.Today).AddDays(7)
            };

            var item = new Domain.Entities.LibraryItem
            {
                Id = 1,
                Title = "Pragmatic",
                Author = "Uncle Bob",
                Pages = 50,
                Type = (int)Domain.Enums.LibraryItemTypeEnum.Book,
                IsBorrowed = false
            };

            var expiredMockItems = GetExpiredMockItems();

            _mockRepository.Setup((r => r.GetMemberById(It.IsAny<int>())))
                .Returns(member);

            _mockRepository.Setup((r => r.GetItemById(It.IsAny<int>())))
                .Returns(item);

            _mockRepository.Setup((r => r.GetMemberBorrowedItems(It.IsAny<int>())))
                .Returns(expiredMockItems);

            //Act
            var result = _libraryService.BorrowItem(1, 1, out var message);

            //Assert
            Assert.False(result);
            Assert.Equal("Members that have overdue items to return can not borrow items.", message);
        }

        [Fact]
        public void WhenAMemberWithAnOverdueSubscriptionBorrowsAnItem_ItShouldReturnFalse()
        {
            //Arrange
            var expiredMockMember = GetExpiredMockMemberSubscription();
            var item = GetMockItem();

            _mockRepository.Setup((r => r.GetMemberById(It.IsAny<int>())))
                .Returns(expiredMockMember);

            _mockRepository.Setup((r => r.GetItemById(It.IsAny<int>())))
                .Returns(item);

            //Act
            var result = _libraryService.BorrowItem(1, 1, out var message);

            //Assert
            Assert.False(result);
            Assert.Equal("This member's suscription is no longer active.", message);
        }

        #endregion BORROW TESTS

        #region SEED MOCK DATA

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

        private static IEnumerable<Domain.Entities.BorrowedItem> GetMockBorrowedItems()
        {
            return new List<Domain.Entities.BorrowedItem>
            {
                new Domain.Entities.BorrowedItem
                {
                    Id = 1,
                    MemberId = 1,
                    LibraryItemId = 1,
                    IsReturned = false,
                    BorrowedDate = DateOnly.FromDateTime(DateTime.Today)
                },
                new Domain.Entities.BorrowedItem
                {
                    Id = 2,
                    MemberId = 2,
                    LibraryItemId = 2,
                    IsReturned = false,
                    BorrowedDate = DateOnly.FromDateTime(DateTime.Today)
                },
                new Domain.Entities.BorrowedItem
                {
                    Id = 3,
                    MemberId = 3,
                    LibraryItemId = 3,
                    IsReturned = false,
                    BorrowedDate = DateOnly.FromDateTime(DateTime.Today)
                }
            };
        }

        private static IEnumerable<Domain.Entities.BorrowedItem> GetExpiredMockItems()
        {
            return new List<Domain.Entities.BorrowedItem>
            {
                new Domain.Entities.BorrowedItem
                {
                    Id = 1,
                    MemberId = 1,
                    LibraryItemId = 1,
                    IsReturned = false,
                    BorrowedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-10)
                },
                new Domain.Entities.BorrowedItem
                {
                    Id = 3,
                    MemberId = 3,
                    LibraryItemId = 3,
                    IsReturned = false,
                    BorrowedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-7)
                }
            };
        }

        private static Member GetMockMember()
        {
            return new Domain.Entities.Member
            {
                Id = 1,
                Name = "Uncle Bob",
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                EndDate = DateOnly.FromDateTime(DateTime.Today).AddDays(7)
            };
        }

        private static LibraryItem GetMockItem()
        {
            return new Domain.Entities.LibraryItem
            {
                Id = 1,
                Title = "Pragmatic",
                Author = "Uncle Bob",
                Pages = 50,
                Type = (int)Domain.Enums.LibraryItemTypeEnum.Book,
                IsBorrowed = false
            };
        }

        private static Member GetExpiredMockMemberSubscription()
        {
            return new Domain.Entities.Member
            {
                Id = 1,
                Name = "Uncle Bob",
                StartDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-30),
                EndDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-23)
            };
        }

        #endregion SEED MOCK DATA
    }
}
