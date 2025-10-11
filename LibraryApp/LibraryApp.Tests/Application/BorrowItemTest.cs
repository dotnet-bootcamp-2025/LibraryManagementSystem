using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Services;
using LibraryApp.Domain.Entities;
using LibraryApp.Domain.Enums;
using Moq;

namespace LibraryApp.Tests.Application;

public class BorrowItemTest
{
    private readonly LibraryService _libraryService;
    private readonly Mock<ILibraryAppRepository> _mockRepository;
    
    public BorrowItemTest()
    {
        _mockRepository = new Mock<ILibraryAppRepository>();
        _libraryService = new LibraryService(_mockRepository.Object); //reemplazar una dependencia creando un objeto
    }

    [Fact]
    public void BorrowItem_WhenMemberDoesNotExist_ReturnsFalseAndMessage()
    {
        //Arrange
        var test = _mockRepository;
        int memberId = 12;
        int libraryItemId = 1;
        test.Setup(r =>r.GetMemberById(memberId))
            .Returns((Domain.Entities.Member?)null);
        //Act
        var result = _libraryService.BorrowItem(memberId, libraryItemId, out var message);
        //Assert
        Assert.False(result);
        Assert.Equal("Member not found.", message);
    }
    [Fact]
    public void BorrowItem_WhenItemDoesNotExist_ReturnsFalseAndMessage()
    {
        var test = _mockRepository;
        int memberId = 1;
        int libraryItemId = 15;
        
        var member = new Domain.Entities.Member {Id = memberId, Name = "Max"};
        test.Setup(r =>r.GetMemberById(memberId)).Returns(member);
        test.Setup(r =>r.GetLibraryItemById(libraryItemId))
            .Returns((Domain.Entities.LibraryItem?)null);
        //Act
        var result = _libraryService.BorrowItem(memberId, libraryItemId, out var message);
        Assert.False(result);
        Assert.Equal("Item not found.", message);
        test.Verify(r => r.GetLibraryItemById(libraryItemId), Times.Once);
        test.Verify(r => r.GetMemberById(memberId), Times.Once);
    }

    [Fact]
    public void BorrowItem_WhenItemExists_ReturnsTrueAndMessage()
    {
        //Arrange
        var test = _mockRepository;
        int memberId = 1;
        int libraryItemId = 1;
        
        var member = new Domain.Entities.Member {Id = memberId, Name = "Max"};
        var item = new Domain.Entities.LibraryItem
        {
            Title = "Blue Deamon",
            Author = "John Doe",
            Pages = 50,
            Type = (int)LibraryItemTypeEnum.Book,
            IsBorrowed = false
        };
        test.Setup(r =>r.GetMemberById(memberId))
            .Returns(new Domain.Entities.Member {Id = memberId, Name = "Max"});
        test.Setup(r => r.GetLibraryItemById(libraryItemId))
            .Returns(item);
        //Act    
        var result = _libraryService.BorrowItem(memberId, libraryItemId, out var message);
        //Assert
        Assert.True(result);
        Assert.NotNull(result);
    }
   
    [Fact]
    public void BorrowItem_WhenIsBorrowedIsTrue_ThenCannotBeBorrowed()
    {
        //Arrange
        var test = _mockRepository;
        int memberId = 1;
        int libraryItemId = 1;
        var member = new Domain.Entities.Member {Id = memberId, Name = "Max"};
        var item = new Domain.Entities.LibraryItem
        {
            Title = "Blue Deamon",
            Author = "John Doe",
            Pages = 50,
            Type = (int)LibraryItemTypeEnum.Book,
            IsBorrowed = true
        };
        
        test.Setup(r =>r.GetMemberById(memberId))
            .Returns(new Domain.Entities.Member {Id = memberId, Name = "Max"});
        test.Setup(r => r.GetLibraryItemById(libraryItemId))
            .Returns(item);
        //Act
        var result = _libraryService.BorrowItem(memberId, libraryItemId, out var message);
        //Assert
        Assert.False(result);
        Assert.Equal($"'{item.Title}' is already borrowed.", message);
    }
    
    [Fact]
    public void BorrowItem_ShouldFail_WhenMemberHasAlreadyThreeItemsBorrowed()
    {
        // Arrange
        var test = _mockRepository;
        int memberId = 1;
        int libraryItemId = 4;
        var member = new Domain.Entities.Member {Id = memberId, Name = "Test Member"};
        var item = new Domain.Entities.LibraryItem
        {
            Title = "Blue Deamon",
            Author = "John Doe",
            Pages = 50,
            Type = (int)LibraryItemTypeEnum.Book,
            IsBorrowed = true
        };
        test.Setup(r => r.GetMemberById(It.IsAny<int>())).Returns(new Member { Id = 1, Name = "Test Member" });
        test.Setup(r => r.GetLibraryItemById(It.IsAny<int>()))
            .Returns(new LibraryItem { Id = 4, Title = "Blue Deamon" , IsBorrowed = false});
    
        var borrowedItems = new List<BorrowedItem>
        {
            new BorrowedItem { 
                MemberId = 1, 
                LibraryItemId = 1, 
                Active = true,
                LibraryItem = new LibraryItem { Id = 1, Title = "Book 1", Type = 1, IsBorrowed = true } 
            },
            new BorrowedItem { 
                MemberId = 1, 
                LibraryItemId = 2, 
                Active = true,
                LibraryItem = new LibraryItem { Id = 2, Title = "Book 2", Type = 1, IsBorrowed = true } 
            },
            new BorrowedItem { 
                MemberId = 1, 
                LibraryItemId = 3, 
                Active = true,
                LibraryItem = new LibraryItem { Id = 3, Title = "Book 3", Type = 1, IsBorrowed = true } 
            }
        };
        
        test.Setup(r => r.GetAllBorrowedItems()).Returns(borrowedItems);
    
        var service = new LibraryService(test.Object);
    
        // Act
        var result = service.BorrowItem(1, 4, out string msg);
    
        // Assert
        Assert.False(result);
        Assert.Equal("Test Member cannot borrow more than 3 items at the same time.", msg);
        test.Verify(r => r.GetMemberById(It.IsAny<int>()), Times.Once);
        test.Verify(r => r.GetLibraryItemById(It.IsAny<int>()), Times.Once);
    }
    
}