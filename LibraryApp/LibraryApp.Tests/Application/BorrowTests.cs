using LibraryApp.Application.Abstraction;
using LibraryApp.Application.Services;
using Moq;


namespace LibraryApp.Tests.Application
{
    public class BorrowTests
    {
        private readonly Mock<ILibraryAppRepository> _mockRepository;
        private readonly LibraryService _libraryService;

        public BorrowTests()
        {
            _mockRepository = new Mock<ILibraryAppRepository>();
            _libraryService = new LibraryService(_mockRepository.Object);
        }
        [Fact]
        public void WhenAnItemIsBorrowed_ValidateHappyPath()
        {
            //arrange 
            var MemberId = 1;
            var ItemId = 1;

            var member = new Domain.Entities.Member
            {
                Id = MemberId,
                Name = "Alice",
                MembershipStartDate = DateTime.Now.AddMonths(-1),
                MembershipEndDate = DateTime.Now.AddMonths(11)
            };
            var item = new Domain.Entities.LibraryItem
            {
                Id = ItemId,
                Title = "The Great Gatsby",
                IsBorrowed = false,
                Type = (int)LibraryApp.Domain.Enums.LibraryItemTypeEnum.Book,
                Author = "F. Scott Fitzgerald",
                Pages = 180
            };
            _mockRepository.Setup(r => r.GetMemberById(MemberId)).Returns(member);
            _mockRepository.Setup(r => r.GetLibraryItemById(ItemId)).Returns(item);
            _mockRepository.Setup(r => r.GetActiveBorrowedItemsByMember(MemberId)).Returns(new List<Domain.Entities.BorrowedItem>());
            _mockRepository.Setup(r => r.AddBorrowedItem(It.IsAny<Domain.Entities.BorrowedItem>())).Verifiable();
            _mockRepository.Setup(r => r.UpdateLibraryItem(It.IsAny<Domain.Entities.LibraryItem>())).Verifiable();

            //act
            var result = _libraryService.BorrowItem(MemberId, ItemId, out var message);
            //assert

            Assert.True(result);
            Assert.Contains("borrowed", message.ToLower());
            _mockRepository.Verify(r => r.UpdateLibraryItem(It.Is<Domain.Entities.LibraryItem>(i => i.Id == ItemId && i.IsBorrowed == true)), Times.Once);
            _mockRepository.Verify(r => r.AddBorrowedItem(It.Is<Domain.Entities.BorrowedItem>(b => b.MemberId == MemberId &&
                b.LibraryItemId == ItemId &&
                b.IsActive == true &&
                b.DueDate > b.BorrowedDate)), Times.Once);
        }
        [Fact]
        public void WhenAnItemIsBorrowed_ValidateWhenAnItemDoesNotExist()
        {
            var LibraryItemId = 1;
            var MemberId = 1;
            _mockRepository.Setup(r => r.GetLibraryItemById(LibraryItemId)).Returns((Domain.Entities.LibraryItem?)null);
            _mockRepository.Setup(r => r.GetMemberById(MemberId)).Returns(new Domain.Entities.Member
            {
                Id = MemberId,
                Name = "Alice",
                MembershipStartDate = DateTime.Now.AddMonths(-1),
                MembershipEndDate = DateTime.Now.AddMonths(11)
            });
            var result = _libraryService.BorrowItem(MemberId, LibraryItemId, out var message);
            Assert.False(result);
            Assert.Equal("Item not found.", message);

        }
        [Fact]
        public void WhenAnItemIsBorrowed_ValidateWhenAMemberDoesNotExist()
        {
            var LibraryItemId = 1;
            var MemberId = 1;
            var item = new Domain.Entities.LibraryItem
            {
                Id = LibraryItemId,
                Title = "The Great Gatsby",
                IsBorrowed = false,
                Type = (int)LibraryApp.Domain.Enums.LibraryItemTypeEnum.Book,
                Author = "F. Scott Fitzgerald",
                Pages = 180
            };
            _mockRepository.Setup(r => r.GetLibraryItemById(LibraryItemId)).Returns(item);
            _mockRepository.Setup(r => r.GetMemberById(MemberId)).Returns((Domain.Entities.Member?)null);
            var result = _libraryService.BorrowItem(MemberId, LibraryItemId, out var message);
            Assert.False(result);
            Assert.Equal("Member not found.", message);
        }

    }
}
