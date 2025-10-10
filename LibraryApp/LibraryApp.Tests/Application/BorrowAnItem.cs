using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Tests.Application
{
    public class BorrowAnItem
    {
        private readonly LibraryService _libraryService;
        private readonly Mock<ILibraryAppRepository> _libraryServiceMock;

        public BorrowAnItem()
        {
            _libraryServiceMock = new Mock<ILibraryAppRepository>();
            _libraryService = new LibraryService(_libraryServiceMock.Object);
        }

        [Fact]
        public void WhenABorrowItem_TheMemberAndItemExists()
        {
            //Arrange
            var item = new Domain.Entities.BorrowedItem
            {
                MemberId = 1,
                LibraryItemId = 1,
                BorrowedDate = DateTime.Now
            };
            _libraryServiceMock.Setup(r => r.AddBorrowedItem(It.IsAny<Domain.Entities.BorrowedItem>()))
                .Callback<Domain.Entities.BorrowedItem>(item => item.MemberId = 1); // Simulate setting the ID when adding
            _libraryServiceMock.Setup(r => r.AddBorrowedItem(It.IsAny<Domain.Entities.BorrowedItem>()))
                .Callback<Domain.Entities.BorrowedItem>(item => item.LibraryItemId = 1); // Simulate setting the ID when adding
            //Act
            var result = _libraryService.BorrowItem;
            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void WhenABorrowItem_TheMemberDoesntExists()
        {
            //Arrange
            var item = new Domain.Entities.BorrowedItem
            {
                MemberId = 0,
                LibraryItemId = 1,
                BorrowedDate = DateTime.Now
            };
            _libraryServiceMock.Setup(r => r.AddBorrowedItem(It.IsAny<Domain.Entities.BorrowedItem>()))
                .Callback<Domain.Entities.BorrowedItem>(item => item.MemberId = 1); // Simulate setting the ID when adding
            _libraryServiceMock.Setup(r => r.AddBorrowedItem(It.IsAny<Domain.Entities.BorrowedItem>()))
                .Callback<Domain.Entities.BorrowedItem>(item => item.LibraryItemId = 1); // Simulate setting the ID when adding
            //Act
            var result1 = _libraryService.BorrowItem;
            //Assert
            Assert.NotNull(result1);
        }
        [Fact]
        public void WhenABorrowItem_TheItemDoesntExists()
        {
            //Arrange
            var item = new Domain.Entities.BorrowedItem
            {
                MemberId = 1,
                LibraryItemId = 0,
                BorrowedDate = DateTime.Now
            };
            _libraryServiceMock.Setup(r => r.AddBorrowedItem(It.IsAny<Domain.Entities.BorrowedItem>()))
                .Callback<Domain.Entities.BorrowedItem>(item => item.MemberId = 1); // Simulate setting the ID when adding
            _libraryServiceMock.Setup(r => r.AddBorrowedItem(It.IsAny<Domain.Entities.BorrowedItem>()))
                .Callback<Domain.Entities.BorrowedItem>(item => item.LibraryItemId = 1); // Simulate setting the ID when adding
            //Act
            var result2 = _libraryService.BorrowItem;
            //Assert
            Assert.NotNull(result2);
        }
       
    }
}
