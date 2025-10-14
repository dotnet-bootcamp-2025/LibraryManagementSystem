using LibraryApp.Application.Abstraction;
using LibraryApp.Application.Services;
using LibraryApp.Domain.Entities;
using Moq;

namespace LibraryApp.Application.UnitTests
{
    public class LibraryService_BorrowingTests
    {
        private readonly Mock<ILibraryAppRepository> _mockRepository;
        private readonly LibraryService _libraryService;

        public LibraryService_BorrowingTests()
        {
            _mockRepository = new Mock<ILibraryAppRepository>();
            _libraryService = new LibraryService(_mockRepository.Object);
        }

        [Fact]
        public void BorrowItem_WhenAllConditionsAreValid_ShouldSucceed()
        {
            var memberId = 1;
            var itemId = 1;

            var fakeMember = new Member
            {
                Id = memberId,
                Name = "Gadiel Mar",
                MembershipEndDate = DateTime.UtcNow.AddYears(1),
                BorrowedItems = new List<BorrowedItem>()
            };

            var fakeLibraryItem = new Domain.Entities.LibraryItem { Id = itemId, Title = "Harry Potter", IsBorrowed = false };

            _mockRepository.Setup(repo => repo.GetMemberById(memberId)).Returns(fakeMember);
            _mockRepository.Setup(repo => repo.GetLibraryItemById(itemId)).Returns(fakeLibraryItem);

            var success = _libraryService.BorrowItem(memberId, itemId, out var message, out var formattedReturnDate);

            Assert.True(success);
            Assert.NotNull(formattedReturnDate);

            _mockRepository.Verify(repo => repo.AddBorrowedItem(It.IsAny<BorrowedItem>()), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public void BorrowItem_WhenMemberHasReachedBorrowLimit_ShouldFail()
        {
            var memberId = 1;
            var itemIdToBorrow = 4;

            var fakeMemberWithMaxLoans = new Member
            {
                Id = memberId,
                Name = "Member with Max Loans",
                MembershipEndDate = DateTime.UtcNow.AddYears(1),
                BorrowedItems = new List<BorrowedItem>
                {
                    new BorrowedItem { IsActive = true, LibraryItem = new LibraryItem { Title = "Book 1" } },
                    new BorrowedItem { IsActive = true, LibraryItem = new LibraryItem { Title = "Book 2" } },
                    new BorrowedItem { IsActive = true, LibraryItem = new LibraryItem { Title = "Book 3" } }
                }
            };

            _mockRepository.Setup(repo => repo.GetMemberById(memberId)).Returns(fakeMemberWithMaxLoans);

            var success = _libraryService.BorrowItem(memberId, itemIdToBorrow, out var message, out var formattedReturnDate);

            Assert.False(success);
            Assert.Contains("Cannot borrow more than 3 items", message);

            _mockRepository.Verify(repo => repo.AddBorrowedItem(It.IsAny<BorrowedItem>()), Times.Never);
        }

        [Fact]
        public void BorrowItem_WhenMemberDoesNotExist_ShouldFail()
        {
            _mockRepository.Setup(repo => repo.GetMemberById(It.IsAny<int>())).Returns((Member?)null);

            var success = _libraryService.BorrowItem(1, 1, out var message, out var formattedReturnDate);

            Assert.False(success);
            Assert.Equal("Member not found.", message);
            _mockRepository.Verify(repo => repo.SaveChanges(), Times.Never);
        }

        [Fact]
        public void BorrowItem_WhenMembershipIsExpired_ShouldFail()
        {
            var memberId = 1;
            var itemId = 1;

            var expiredMember = new Member
            {
                Id = memberId,
                Name = "Expired Member",
                MembershipEndDate = DateTime.UtcNow.AddDays(-1)
            };

            _mockRepository.Setup(repo => repo.GetMemberById(memberId)).Returns(expiredMember);

            var success = _libraryService.BorrowItem(memberId, itemId, out var message, out var formattedReturnDate);

            Assert.False(success);
            Assert.Contains("membership has expired", message);
            _mockRepository.Verify(repo => repo.SaveChanges(), Times.Never);
        }

        [Fact]
        public void BorrowItem_WhenMemberHasOverdueItems_ShouldFail()
        {
            var memberId = 1;
            var itemId = 2;

            var memberWithOverdueItem = new Member
            {
                Id = memberId,
                Name = "Member with Overdue Item",
                MembershipEndDate = DateTime.UtcNow.AddYears(1),
                BorrowedItems = new List<BorrowedItem>
                {
                   new BorrowedItem { IsActive = true, ReturnDate = DateTime.UtcNow.AddDays(-5), LibraryItem = new LibraryItem {Title = "Overdue Book"}
                }
            }
            };

            _mockRepository.Setup(repo => repo.GetMemberById(memberId)).Returns(memberWithOverdueItem);
            _mockRepository.Setup(repo => repo.GetLibraryItemById(itemId)).Returns(new LibraryItem
            {
                Id = itemId,
                Title = "Available Book",
                IsBorrowed = false
            });

            var success = _libraryService.BorrowItem(memberId, itemId, out var message, out var formattedReturnDate);

            Assert.False(success);
            Assert.Contains("expired items that must be returned first", message);
            _mockRepository.Verify(repo => repo.SaveChanges(), Times.Never);
        }

        [Fact]
        public void BorrowItem_WhenItemDoesNotExist_ShouldFail()
        {
            var memberId = 1;
            var itemId = 99;
            var validMember = new Member
            {
                Id = memberId,
                Name = "Valid Member",
                MembershipEndDate = DateTime.UtcNow.AddYears(1),
                BorrowedItems = new List<BorrowedItem>()
            };

            _mockRepository.Setup(repo => repo.GetMemberById(memberId)).Returns(validMember);

            _mockRepository.Setup(repo => repo.GetLibraryItemById(itemId)).Returns((LibraryItem?)null);

            var success = _libraryService.BorrowItem(memberId, itemId, out var message, out var formattedReturnDate);

            Assert.False(success);
            Assert.Equal("Item not found.", message);
            _mockRepository.Verify(repo => repo.SaveChanges(), Times.Never);
        }

        [Fact]
        public void BorrowItem_WhenItemIsAlreadyBorrowed_ShouldFail()
        {
            var memberId = 1;
            var itemId = 1;
            var validMember = new Member
            {
                Id = memberId,
                Name = "Valid Member",
                MembershipEndDate = DateTime.UtcNow.AddYears(1),
                BorrowedItems = new List<BorrowedItem>()
            };

            var borrowedItemEntity = new Domain.Entities.LibraryItem { Id = itemId, IsBorrowed = true, Title = "Borrowed Book" };

            _mockRepository.Setup(repo => repo.GetMemberById(memberId)).Returns(validMember);
            _mockRepository.Setup(repo => repo.GetLibraryItemById(itemId)).Returns(borrowedItemEntity);
            _mockRepository.Setup(repo => repo.GetBorrowedItemByLibraryItemId(itemId)).Returns(new BorrowedItem { ReturnDate = DateTime.UtcNow.AddDays(2) });

            var success = _libraryService.BorrowItem(memberId, itemId, out var message, out var formattedReturnDate);

            Assert.False(success);
            Assert.Contains("days left for its return", message);
            _mockRepository.Verify(repo => repo.SaveChanges(), Times.Never);
        }
    }
}