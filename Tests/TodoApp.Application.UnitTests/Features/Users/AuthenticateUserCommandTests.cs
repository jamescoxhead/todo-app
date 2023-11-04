using FluentAssertions.Execution;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TodoApp.Application.Configuration.Models;
using TodoApp.Application.Users.Commands;
using TodoApp.Infrastructure.Identity.Models;

namespace TodoApp.Application.UnitTests.Features.Users;

public class AuthenticateUserCommandTests
{
    private readonly Mock<UserManager<ApplicationUser>> mockUserManager = TestHelpers.CreateMockUserManager<ApplicationUser>();
    private readonly JwtOptions jwtOptions = new()
    {
        Audience = "http://audience",
        ExpiresHours = 24,
        Issuer = "http://issuer",
        Secret = Guid.NewGuid().ToString()
    };

    [SetUp]
    public void TestCastSetUp() => this.mockUserManager.Reset();

    [Test]
    public void Contructor_ShouldThrowNullReferenceException_WhenUserManagerIsNull()
    {
        var action = () => new AuthenticateUserCommandHandler(null!, this.CreateJwtOptions());

        action.Should().ThrowExactly<ArgumentNullException>().WithParameterName("userManager");
    }

    [Test]
    public void Contructor_ShouldThrowNullReferenceException_WhenJwtOptionsIsNull()
    {
        var action = () => new AuthenticateUserCommandHandler(this.mockUserManager.Object, null!);

        action.Should().ThrowExactly<ArgumentNullException>().WithParameterName("jwtOptions");
    }

    [Test]
    public void Constructor_ShouldInstantiate_WithValidParameters()
    {
        var sut = this.CreateSystemUnderTest(this.mockUserManager.Object);

        sut.Should().NotBeNull();
    }

    [Test]
    public async Task Authenticate_ShouldNotReturnToken_WhenUserInvalid()
    {
        // Arrange
        this.mockUserManager.Setup(m => m.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(false);
        var sut = this.CreateSystemUnderTest(this.mockUserManager.Object);

        var authenticateUser = new AuthenticateUserCommand
        {
            Username = "test",
            Password = "password"
        };

        // Act
        var result = await sut.Handle(authenticateUser, CancellationToken.None);

        // Assert
        result.IsAuthenticatedUser.Should().BeFalse();
    }

    [Test]
    public async Task Authenticate_ReturnsToken_WhenUserValid()
    {
        // Arrange
        var authenticateUser = new AuthenticateUserCommand
        {
            Username = "test",
            Password = "password"
        };

        this.mockUserManager.Setup(m => m.FindByNameAsync(authenticateUser.Username).Result).Returns(new ApplicationUser { Email = authenticateUser.Username, UserName = authenticateUser.Username });
        this.mockUserManager.Setup(m => m.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(true);
        var sut = this.CreateSystemUnderTest(this.mockUserManager.Object);

        // Act
        var result = await sut.Handle(authenticateUser, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.IsAuthenticatedUser.Should().BeTrue();
            result.Token.Should().NotBeNullOrEmpty();
            result.Expiry.Should().BeSameDateAs(DateTime.Now.AddHours(this.jwtOptions.ExpiresHours));
        }
    }

    private AuthenticateUserCommandHandler CreateSystemUnderTest(UserManager<ApplicationUser> userManager) => new(userManager, this.CreateJwtOptions());
    private IOptions<JwtOptions> CreateJwtOptions() => Options.Create(this.jwtOptions);
}
