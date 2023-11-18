using AutoMapper;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Identity;
using TodoApp.Application.Exceptions;
using TodoApp.Application.Mapping;
using TodoApp.Application.Users.Commands;
using TodoApp.Infrastructure.Identity.Models;

namespace TodoApp.Application.UnitTests.Features.Users;

[TestFixture]
public class CreateUserCommandTests
{
    private IMapper mapper;
    private UserManager<ApplicationUser> mockUserManager = TestHelpers.CreateMockUserManager<ApplicationUser>();

    [OneTimeSetUp]
    public void TestFixtureSetUp()
    {
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<UserMappingProfile>());
        this.mapper = mapperConfig.CreateMapper();
    }

    [SetUp]
    public void TestCastSetUp() => this.mockUserManager = TestHelpers.CreateMockUserManager<ApplicationUser>();

    [TearDown]
    public void TestCaseTearDown() => this.mockUserManager.Dispose();

    [OneTimeTearDown]
    public void TestFixtureTearDown() => this.mockUserManager.Dispose();

    [Test]
    public void Contructor_ShouldThrowNullReferenceException_WhenUserManagerIsNull()
    {
        var action = () => new CreateUserCommandHandler(null!, this.mapper);

        action.Should().ThrowExactly<ArgumentNullException>().WithParameterName("userManager");
    }

    [Test]
    public void Contructor_ShouldThrowNullReferenceException_WhenMapperIsNull()
    {
        var action = () => new CreateUserCommandHandler(this.mockUserManager, null!);

        action.Should().ThrowExactly<ArgumentNullException>().WithParameterName("mapper");
    }

    [Test]
    public void Constructor_ShouldInstantiate_WithValidParameters()
    {
        var sut = this.CreateSystemUnderTest(this.mockUserManager);

        sut.Should().NotBeNull();
    }

    [Test]
    public async Task CreateUser_ShouldCreateNewUser()
    {
        // Arrange
        this.mockUserManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>())
                            .ReturnsForAnyArgs(IdentityResult.Success);

        var sut = this.CreateSystemUnderTest(this.mockUserManager);

        var newUser = new CreateUserCommand
        {
            Email = "test@test.com",
            Password = "test password"
        };

        // Act
        var result = await sut.Handle(newUser, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Email.Should().Be(newUser.Email);
            result.UserName.Should().Be(newUser.Email);
        }
    }

    [Test]
    public async Task CreateUser_ShouldThrowIdentityException_WhenErrorOccurs()
    {
        // Arrange
        this.mockUserManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>())
                            .ReturnsForAnyArgs(IdentityResult.Failed(new IdentityError { Code = "101", Description = "Something went wrong" }));

        var sut = this.CreateSystemUnderTest(this.mockUserManager);

        var newUser = new CreateUserCommand
        {
            Email = "test@test.com",
            Password = "test password"
        };

        // Act
        var createAction = async () => await sut.Handle(newUser, CancellationToken.None);

        // Assert
        await createAction.Should().ThrowExactlyAsync<IdentityException>();
    }

    private CreateUserCommandHandler CreateSystemUnderTest(UserManager<ApplicationUser> userManager) => new(userManager, this.mapper);
}
