using LibraryApp.Application.Abstraction;
using LibraryApp.Application.Services;
using LibraryApp.Domain.Entities;
using Moq;

namespace LibraryApp.Application.UnitTests
{
    public class LibraryServiceTests
    {
        private readonly LibraryService _libraryService;
        private readonly Mock<ILibraryAppRepository> _mockRepository;
        public LibraryServiceTests()
        {
            _mockRepository = new Mock<ILibraryAppRepository>();
            _libraryService = new LibraryService(_mockRepository.Object);
        }

        [Fact]
        public void GetAllMembers_WhenMembersExist_ShouldReturnMemberList()
        {
            var fakeMemberEntities = new List<Domain.Entities.Member>
            {
                new Domain.Entities.Member { Id = 1, Name = "Alice" },
                new Domain.Entities.Member { Id = 2, Name = "Bob" }
            };

            _mockRepository.Setup(repo => repo.GetAllMembers()).Returns(fakeMemberEntities);

            var result = _libraryService.GetAllMembers();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, m => m.Name == "Alice");
        }

        [Fact]
        public void GetAllMembers_WhenNoMembersExist_ShouldReturnEmptyList()
        {

            _mockRepository.Setup(repo => repo.GetAllMembers()).Returns(new List<Domain.Entities.Member>());

            var result = _libraryService.GetAllMembers();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetMemberActiveLoans_WhenMemberHasActiveLoans_ShouldReturnLoanDetailsDtoList()
        {
            var memberId = 1;
            var fakeLoans = new List<BorrowedItem>
            {
                new BorrowedItem
                {
                    MemberId = memberId,
                    IsActive = true,
                    ReturnDate = DateTime.UtcNow.AddDays(2),
                    LibraryItem = new LibraryItem { Title = "Harry Potter" }
                },
                new BorrowedItem
                {
                    MemberId = memberId,
                    IsActive = true,
                    ReturnDate = DateTime.UtcNow.AddDays(3),
                    LibraryItem = new LibraryItem { Title = "Inteligencia Emocional" }
                }
            };

            _mockRepository.Setup(repo => repo.GetActiveLoansByMemberId(memberId)).Returns(fakeLoans);

            var result = _libraryService.GetMemberActiveLoans(memberId);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, dto => dto.ItemTitle == "Harry Potter");
        }

        [Fact]
        public void GetMemberActiveLoans_WhenMemberHasNoActiveLoans_ShouldReturnEmptyList()
        {
            var memberId = 2;

            _mockRepository.Setup(repo => repo.GetActiveLoansByMemberId(memberId)).Returns(new List<BorrowedItem>());

            var result = _libraryService.GetMemberActiveLoans(memberId);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void RegisterMember_WithValidName_ShouldCallRepositoryMethods()
        {
            var memberName = "Gadiel Mar";

            _mockRepository.Setup(r => r.AddMember(It.IsAny<Member>()))
                        .Callback<Member>(member => member.Id = 1);

            var result = _libraryService.RegisterMember(memberName);

            Assert.NotNull(result);
            Assert.Equal(memberName, result.Name);
            Assert.Equal(1, result.Id);

            _mockRepository.Verify(repo => repo.AddMember(It.IsAny<Member>()), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChanges(), Times.Once);
        }
    }
}