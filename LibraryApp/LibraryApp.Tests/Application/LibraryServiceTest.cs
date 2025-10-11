using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Services;
using LibraryApp.Domain.Entities;
using LibraryApp.Domain.Enums;
using Moq;

namespace LibraryApp.Tests.Application;

public class LibraryServiceTest
{
    private readonly LibraryService _libraryService;
    private readonly Mock<ILibraryAppRepository> _mockRepository; //emula el comportamiento de las dependencias externas y asi aislar lo más posible

    public LibraryServiceTest()
    {
        _mockRepository = new Mock<ILibraryAppRepository>();
        _libraryService = new LibraryService(_mockRepository.Object); //reemplazar una dependencia creando un objeto
    }
    [Fact]
    public void WhenABookIsAdded_ThenItShouldBeCreated()
    {
        //Arrange
        var blueDeamonBook = new Domain.Entities.LibraryItem
        {
            Title = "Blue Deamon",
            Author = "John Doe",
            Pages = 50,
            Type = (int)LibraryItemTypeEnum.Book,
            IsBorrowed = false
        };
        //emular o simular el comportamiento
        _mockRepository
            .Setup(r => r.AddLibraryItem(It.IsAny<Domain.Entities.LibraryItem>()))
            .Callback<Domain.Entities.LibraryItem>(item =>
            {
                item.Id = 1; //simulate assing id
            });
        //Act 
        var result = _libraryService.AddBook(
            blueDeamonBook.Title,
            blueDeamonBook.Author,
            blueDeamonBook.Pages.Value);
        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }
    //Add Unit test for GetAllLibraryItems
    [Fact] //extract method
    public void WhenGetAllLibraryItemsIsCalled_ThenItShouldBeReturned()
    {
        //Arrange
        var items = new List<Domain.Entities.LibraryItem>
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
        _mockRepository
            .Setup(r => r.GetAllLibraryItems())
            .Returns(items);
        //Act    
        var result = _libraryService.GetAllLibraryItems();
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        
    }
    //-----------------------------------------------------------------
    //Create a Unit Test to Register a Member
    [Fact]
    public void WhenAMemberIsRegistered_ThenItShouldBeCreated()
    {
        //Arrange
        var member = new Domain.Entities.Member
        {
            Name = "Max Doe"
        };
        
        _mockRepository
            .Setup(r => r.AddMember(It.IsAny<Domain.Entities.Member>()))
            .Callback <Domain.Entities.Member>(item =>
            {
                item.Id = 1;
            });
        //Act
        var result = _libraryService.RegisterMember(member.Name);
        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }
    [Fact]
    public void BorrowItem_ShouldSetActiveTrue_WhenNewBorrowedItemIsCreated()
    {
        // Arrange
        var mockRepo = new Mock<ILibraryAppRepository>();
        mockRepo.Setup(r => r.GetMemberById(It.IsAny<int>())).Returns(new Member { Id = 1, Name = "Test Member" });
        mockRepo.Setup(r => r.GetLibraryItemById(It.IsAny<int>())).Returns(new LibraryItem { Id = 1, Title = "Test Book", IsBorrowed = false });

        BorrowedItem capturedBorrowedItem = null;
        mockRepo.Setup(r => r.AddBorrowedItem(It.IsAny<BorrowedItem>()))
            .Callback<BorrowedItem>(b => capturedBorrowedItem = b);

        var service = new LibraryService(mockRepo.Object);

        // Act
        var result = service.BorrowItem(1, 1, out string msg);

        // Assert
        Assert.True(result);
        Assert.NotNull(capturedBorrowedItem);
        Assert.True(capturedBorrowedItem.Active); // ✅ Verify Active is true
        Assert.Equal(1, capturedBorrowedItem.MemberId);
        Assert.Equal(1, capturedBorrowedItem.LibraryItemId);
    }

}