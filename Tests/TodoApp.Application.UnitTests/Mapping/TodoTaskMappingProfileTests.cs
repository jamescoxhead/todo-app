using AutoMapper;
using FluentAssertions.Execution;
using TodoApp.Application.Dtos;
using TodoApp.Application.Mapping;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.UnitTests.Mapping;

[TestFixture]
public class TodoTaskMappingProfileTests
{
    private IMapper mapper;
    private MapperConfiguration mapperConfiguration;

    [OneTimeSetUp]
    public void TestFixtureSetUp()
    {
        this.mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<TodoTaskMappingProfile>());
        this.mapper = this.mapperConfiguration.CreateMapper();
    }

    [Test]
    public void IsValidConfiguration() => this.mapperConfiguration.AssertConfigurationIsValid();

    [Test]
    public void CanMap_CreateTodoTaskDto_To_TodoTask()
    {
        // Arrange
        var source = new CreateTodoTaskDto
        {
            Description = "description",
            DueDate = DateTime.Now
        };

        // Act
        var result = this.mapper.Map<TodoTask>(source);

        // Assert
        using (new AssertionScope())
        {
            result.Description.Should().Be(source.Description);
            result.DueDate.Should().Be(source.DueDate);
        }
    }

    [Test]
    public void CanMap_TodoTask_To_TodoTaskDto()
    {
        var source = new TodoTask
        {
            Id = 123,
            Description = "description",
            DueDate = DateTime.Now.AddDays(5),
            IsComplete = true
        };

        // Act
        var result = this.mapper.Map<TodoTaskDto>(source);

        // Assert
        using (new AssertionScope())
        {
            result.Description.Should().Be(source.Description);
            result.DueDate.Should().Be(source.DueDate);
            result.IsComplete.Should().Be(source.IsComplete);
            result.Id.Should().Be(source.Id);
        }
    }

    [Test]
    public void CanMap_TodoTaskDto_To_TodoTask()
    {
        var source = new TodoTaskDto
        {
            Id = 123,
            Description = "description",
            DueDate = DateTime.Now.AddDays(5),
            IsComplete = true
        };

        // Act
        var result = this.mapper.Map<TodoTask>(source);

        // Assert
        using (new AssertionScope())
        {
            result.Description.Should().Be(source.Description);
            result.DueDate.Should().Be(source.DueDate);
            result.IsComplete.Should().Be(source.IsComplete);
            result.Id.Should().Be(source.Id);
        }
    }
}
