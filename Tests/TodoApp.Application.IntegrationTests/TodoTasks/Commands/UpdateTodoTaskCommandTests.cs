using AutoMapper;
using FluentAssertions.Execution;
using TodoApp.Application.Exceptions;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Mapping;
using TodoApp.Application.TodoTasks.Commands;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.IntegrationTests.TodoTasks.Commands;

[TestFixture]
public class UpdateTodoTaskCommandTests : SqlLiteTestBase
{
    private IMapper mapper;

    [OneTimeSetUp]
    public void TestFixtureSetUp()
    {
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<TodoTaskMappingProfile>());
        this.mapper = mapperConfig.CreateMapper();
    }

    [Test]
    public void Contructor_ShouldThrowNullReferenceException_WhenDbContextIsNull()
    {
        var action = () => new UpdateTodoTaskCommandHandler(null!, this.mapper);

        action.Should().ThrowExactly<ArgumentNullException>().WithParameterName("dbContext");
    }

    [Test]
    public void Contructor_ShouldThrowNullReferenceException_WhenMapperIsNull()
    {
        var action = () => new UpdateTodoTaskCommandHandler(this.CreateDbContext, null!);

        action.Should().ThrowExactly<ArgumentNullException>().WithParameterName("mapper");
    }

    [Test]
    public void Constructor_ShouldInstantiate_WithValidParameters()
    {
        var sut = this.CreateSystemUnderTest(this.CreateDbContext);

        sut.Should().NotBeNull();
    }

    [Test]
    public async Task UpdateTodoTask_ShouldUpdateTodoTask_WhenTodoTaskExists()
    {
        // Arrange
        var dbContext = this.CreateDbContext;
        var sut = this.CreateSystemUnderTest(dbContext);

        var taskToUpdate = new TodoTask
        {
            Description = "Update me",
            DueDate = DateTime.Now.AddDays(5),
            IsComplete = false
        };

        dbContext.TodoTasks.Add(taskToUpdate);
        dbContext.SaveChanges();

        var updateTaskCommand = new UpdateTodoTaskCommand
        {
            TodoTaskId = taskToUpdate.Id,
            IsComplete = true
        };

        // Act
        var result = await sut.Handle(updateTaskCommand, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result!.Id.Should().Be(taskToUpdate.Id);
            result.Description.Should().Be(taskToUpdate.Description);
            result.DueDate.Should().Be(taskToUpdate.DueDate);
            result.IsComplete.Should().Be(updateTaskCommand.IsComplete);
        }

        // Cleanup
        dbContext.TodoTasks.Remove(taskToUpdate);
        dbContext.SaveChanges();
    }

    [Test]
    public async Task UpdateTodoTask_ShouldThrowNotFoundException_WhenTodoTaskNotFound()
    {
        // Arrange
        var dbContext = this.CreateDbContext;
        var sut = this.CreateSystemUnderTest(dbContext);

        var updateTaskCommand = new UpdateTodoTaskCommand
        {
            TodoTaskId = 12,
            IsComplete = true
        };

        // Act
        var action = async () => await sut.Handle(updateTaskCommand, CancellationToken.None);

        // Assert
        await action.Should().ThrowExactlyAsync<NotFoundException>();
    }

    private UpdateTodoTaskCommandHandler CreateSystemUnderTest(ITodoDbContext dbContext) => new(dbContext, this.mapper);
}
