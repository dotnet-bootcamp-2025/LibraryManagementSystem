using System.Runtime.InteropServices.JavaScript;
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
            Name = "Max Doe",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddYears(1)
        };
        
        _mockRepository
            .Setup(r => r.AddMember(It.IsAny<Domain.Entities.Member>()))
            .Callback <Domain.Entities.Member>(item =>
            {
                item.Id = 1;
            });
        //Act
        var result = _libraryService.RegisterMember(member.Name, member.StartDate, member.EndDate);
        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

}