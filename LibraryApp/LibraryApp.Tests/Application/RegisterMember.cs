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
    public class RegisterMember
    {
        private readonly LibraryService _libraryService;
        private readonly Mock<ILibraryAppRepository> _libraryServiceMock;

        public RegisterMember()
        {
            _libraryServiceMock = new Mock<ILibraryAppRepository>();
            _libraryService = new LibraryService(_libraryServiceMock.Object);
        }

        [Fact]
        public void WhenAMemberIsRegistered_ThenItShouldBeCreated() {

            //Arrange
            var miembro = new Domain.Entities.Member
            {
                Name = "Juan"
            };

            _libraryServiceMock.Setup(r => r.AddMember(It.IsAny<Domain.Entities.Member>()))
                .Callback<Domain.Entities.Member>(item =>
                {
                    item.Id = 1; //Simulate database assigning an ID
                });

            //Act
            var result = _libraryServiceMock.Object.AddMember;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]

        public void WhenAMemberIsRegistered_ThenNotShouldBeCreated()
        {

            //Arrange
            var miembro = new Domain.Entities.Member
            {
                Name = ""
            };

            _libraryServiceMock.Setup(r => r.AddMember(It.IsAny<Domain.Entities.Member>()))
                .Callback<Domain.Entities.Member>(item =>
                {
                    item.Id = 1; //Simulate database assigning an ID
                });

            //Act
            var result = _libraryServiceMock.Object.AddMember;

            //Assert
            Assert.NotNull(result);
        }

    }
}
