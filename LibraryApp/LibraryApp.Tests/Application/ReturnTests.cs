using LibraryApp.Application.Abstraction;
using LibraryApp.Application.Services;
using Moq;

namespace LibraryApp.Tests.Application
{
    public class ReturnTests
    {
        private readonly Mock<ILibraryAppRepository> _mockRepository;
        private readonly LibraryService _libraryService;

        public ReturnTests()
        {
            _mockRepository = new Mock<ILibraryAppRepository>();
            _libraryService = new LibraryService(_mockRepository.Object);
        }

        [Fact]
        public void WhenAnItemIsReturned_ThenItShouldBeSoftDeleted()
        {
            // Arrange
            var memberId = 1;
            var itemId = 1;

            var member = new Domain.Entities.Member
            {
                Id = memberId,
                Name = "Alice Johnson",
                MembershipStartDate = DateTime.Now.AddMonths(-1),
                MembershipEndDate = DateTime.Now.AddMonths(11)
            };

            var item = new Domain.Entities.LibraryItem
            {
                Id = itemId,
                Title = "The Great Gatsby",
                IsBorrowed = true, // Currently borrowed
                Type = (int)LibraryApp.Domain.Enums.LibraryItemTypeEnum.Book
            };

            var borrowedItem = new Domain.Entities.BorrowedItem
            {
                Id = 1,
                MemberId = memberId,
                LibraryItemId = itemId,
                IsActive = true, // Active borrow
                BorrowedDate = DateTime.Now.AddDays(-2),
                DueDate = DateTime.Now.AddDays(1)
            };

            _mockRepository.Setup(r => r.GetMemberById(memberId)).Returns(member);
            _mockRepository.Setup(r => r.GetLibraryItemById(itemId)).Returns(item);
            _mockRepository.Setup(r => r.GetBorrowedItem(memberId, itemId))
                .Returns(new List<Domain.Entities.BorrowedItem> { borrowedItem });
            _mockRepository.Setup(r => r.UpdateLibraryItem(It.IsAny<Domain.Entities.LibraryItem>()));
            _mockRepository.Setup(r => r.UpdateBorrowedItem(It.IsAny<Domain.Entities.BorrowedItem>()));

            // Act
            var result = _libraryService.ReturnItem(memberId, itemId, out var message);

            // Assert
            Assert.True(result);
            Assert.Contains("returned", message.ToLower());

            // Verify item marked as not borrowed
            _mockRepository.Verify(r => r.UpdateLibraryItem(It.Is<Domain.Entities.LibraryItem>(
                i => i.Id == itemId && i.IsBorrowed == false)), Times.Once);

            // Verify soft delete (IsActive = false)
            _mockRepository.Verify(r => r.UpdateBorrowedItem(It.Is<Domain.Entities.BorrowedItem>(
                bi => bi.Id == 1 && bi.IsActive == false)), Times.Once);
        }
    }
}