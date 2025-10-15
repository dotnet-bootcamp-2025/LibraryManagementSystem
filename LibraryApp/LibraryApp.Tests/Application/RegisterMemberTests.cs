using LibraryApp.Application.Abstraction;
using LibraryApp.Application.Services;
using Moq;

namespace LibraryApp.Tests.Application
{
    public class RegisterMemberTests
    {
        private readonly Mock<ILibraryAppRepository> _mockRepository;
        private readonly LibraryService _libraryService;

        public RegisterMemberTests()
        {
            _mockRepository = new Mock<ILibraryAppRepository>();
            _libraryService = new LibraryService(_mockRepository.Object);
        }
        [Fact]
        public void WhenAMemberisAdded_RegisterMember()
        {
            var member = new Domain.Entities.Member
            {
                Name = "Bob",
                MembershipStartDate = DateTime.Now,
                MembershipEndDate = DateTime.Now.AddYears(1)
            };
            _mockRepository.Setup(r => r.RegisterMember(It.IsAny<Domain.Entities.Member>()))
                .Callback<Domain.Entities.Member>(m => m.Id = 1); // Simulate DB assigning ID
            var result = _libraryService.RegisterMember(member.Name);
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Bob", result.Name);

        }
    }
}
