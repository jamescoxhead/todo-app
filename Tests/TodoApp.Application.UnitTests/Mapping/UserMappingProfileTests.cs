using AutoMapper;
using FluentAssertions.Execution;
using TodoApp.Application.Mapping;
using TodoApp.Application.Users.Commands;
using TodoApp.Application.Users.Dtos;
using TodoApp.Infrastructure.Identity.Models;

namespace TodoApp.Application.UnitTests.Mapping;

[TestFixture]
public class UserMappingProfileTests
{
    private IMapper mapper;
    private MapperConfiguration mapperConfiguration;

    [OneTimeSetUp]
    public void TestFixtureSetUp()
    {
        this.mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<UserMappingProfile>());
        this.mapper = this.mapperConfiguration.CreateMapper();
    }

    [Test]
    public void IsValidConfiguration() => this.mapperConfiguration.AssertConfigurationIsValid();

    [Test]
    public void CanMap_CreateUserCommand_To_ApplicationUser()
    {
        // Arrange
        var source = new CreateUserCommand
        {
            Email = "test@test.com",
            Password = "Password123"
        };

        // Act
        var result = this.mapper.Map<ApplicationUser>(source);

        // Assert
        using (new AssertionScope())
        {
            result.UserName.Should().Be(source.Email);
            result.Email.Should().Be(source.Email);
            result.PhoneNumber.Should().BeNull();
            result.PhoneNumberConfirmed.Should().BeFalse();
            result.EmailConfirmed.Should().BeFalse();
            result.TwoFactorEnabled.Should().BeFalse();
        }
    }

    [Test]
    public void CanMap_ApplicationUser_To_UserDto()
    {
        // Arrange
        var source = new ApplicationUser
        {
            Id = 100,
            UserName = "test@test.com",
            Email = "test@test.com"
        };

        // Act
        var result = this.mapper.Map<UserDto>(source);

        // Assert
        using (new AssertionScope())
        {
            result.UserName.Should().Be(source.UserName);
            result.Email.Should().Be(source.Email);
            result.Id.Should().Be(source.Id);
        }
    }
}
