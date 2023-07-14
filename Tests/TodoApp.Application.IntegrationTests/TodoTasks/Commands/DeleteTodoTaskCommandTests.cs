using TodoApp.Application.Exceptions;
using TodoApp.Application.Interfaces;
using TodoApp.Application.TodoTasks.Commands;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.IntegrationTests.TodoTasks.Commands;

[TestFixture]
public class DeleteTodoTaskCommandTests : SqlLiteTestBase
{
    [Test]
    public void Contructor_ShouldThrowNullReferenceException_WhenDbContextIsNull()
    {
        var action = () => new DeleteTodoTaskCommandHandler(null!);

        action.Should().ThrowExactly<ArgumentNullException>().WithParameterName("dbContext");
    }

    [Test]
    public void Constructor_ShouldInstantiate_WithValidParameters()
    {
        var sut = CreateSystemUnderTest(this.CreateDbContext);

        sut.Should().NotBeNull();
    }

    [Test]
    public async Task DeleteTodoTask_ShouldDeleteTodoTask_WhenTodoTaskExists()
    {
        // Arrange
        var dbContext = this.CreateDbContext;
        var sut = CreateSystemUnderTest(dbContext);

        var taskToDelete = new TodoTask
        {
            Description = "Delete me"
        };

        dbContext.TodoTasks.Add(taskToDelete);
        dbContext.SaveChanges();

        var deleteCommand = new DeleteTodoTaskCommand { TodoTaskId = taskToDelete.Id };

        // Act
        await sut.Handle(deleteCommand, CancellationToken.None);

        // Assert
        var dbTask = dbContext.TodoTasks.Find(taskToDelete.Id);
        dbTask.Should().BeNull();
    }

    [Test]
    public async Task DeleteTodoTask_ShouldThrowNotFoundException_WhenTodoTaskNotFound()
    {
        // Arrange
        var dbContext = this.CreateDbContext;
        var sut = CreateSystemUnderTest(dbContext);
        var deleteCommand = new DeleteTodoTaskCommand { TodoTaskId = 123 };

        // Act
        var deleteAction = async () => await sut.Handle(deleteCommand, CancellationToken.None);

        // Assert
        await deleteAction.Should().ThrowExactlyAsync<NotFoundException>();
    }

    private static DeleteTodoTaskCommandHandler CreateSystemUnderTest(ITodoDbContext dbContext) => new(dbContext);
}
