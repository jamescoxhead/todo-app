using FluentAssertions.Execution;
using TodoApp.Application.Dtos;
using TodoApp.Application.Services;
using TodoApp.Domain.Entities;
using TodoApp.Infrastructure.Persistence;

namespace TodoApp.Application.IntegrationTests.Services;

public class TodoTaskServiceTests : SqlLiteTestBase
{
    [Test]
    public async Task CreateTodoTask_ShouldCreateNewTodoTask()
    {
        // Arrange
        var dbContext = this.CreateDbContext;
        var sut = CreateSystemUnderTest(dbContext);

        var newTask = new CreateTodoTaskDto
        {
            Description = "Test task",
            DueDate = DateTime.Now.AddDays(1)
        };

        // Act
        var result = await sut.CreateTodoTask(newTask);

        // Assert
        var dbTask = dbContext.TodoTasks.Find(result.Id);

        using (new AssertionScope())
        {
            dbTask.Should().NotBeNull();
            dbTask!.Description.Should().Be(newTask.Description);
            dbTask.DueDate.Should().Be(newTask.DueDate);
            dbTask.IsComplete.Should().BeFalse();
        }

        // Reset
        dbContext.TodoTasks.Remove(dbTask);
        dbContext.SaveChanges();
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

        // Act
        await sut.DeleteTodoTask(taskToDelete.Id);

        // Assert
        var dbTask = dbContext.TodoTasks.Find(taskToDelete.Id);
        dbTask.Should().BeNull();
    }

    [Test]
    public async Task DeleteTodoTask_ShouldThrowException_WhenTodoTaskNotFound()
    {
        // Arrange
        var dbContext = this.CreateDbContext;
        var sut = CreateSystemUnderTest(dbContext);
        var taskId = 123;

        // Act
        var deleteAction = async () => await sut.DeleteTodoTask(taskId);

        // Assert
        await deleteAction.Should().ThrowAsync<Exception>();
    }

    [Test]
    public async Task GetTodoTaskById_ShouldReturnTodoTask_WhenTodoTaskExists()
    {
        // Arrange
        var dbContext = this.CreateDbContext;
        var sut = CreateSystemUnderTest(dbContext);

        var taskToGet = new TodoTask
        {
            Description = "Delete me",
            DueDate = DateTime.Now.AddDays(5),
            IsComplete = true
        };

        dbContext.TodoTasks.Add(taskToGet);
        dbContext.SaveChanges();

        // Act
        var result = await sut.GetTodoTaskById(taskToGet.Id);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result!.Id.Should().Be(taskToGet.Id);
            result.Description.Should().Be(taskToGet.Description);
            result.DueDate.Should().Be(taskToGet.DueDate);
            result.IsComplete.Should().Be(taskToGet.IsComplete);
        }

        // Cleanup
        dbContext.TodoTasks.Remove(taskToGet);
        dbContext.SaveChanges();
    }

    [Test]
    public async Task GetTodoTaskById_ShouldReturnNull_WhenTodoTaskNotFound()
    {
        // Arrange
        var dbContext = this.CreateDbContext;
        var sut = CreateSystemUnderTest(dbContext);
        var taskId = 123;

        // Act
        var result = await sut.GetTodoTaskById(taskId);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task GetTodoTasks_ShouldReturnTodoTasks()
    {
        // Arrange
        var dbContext = this.CreateDbContext;
        var sut = CreateSystemUnderTest(dbContext);

        var tasks = new List<TodoTask>
        {
            new TodoTask {Description = "Task 1"},
            new TodoTask {Description = "Task 2"},
            new TodoTask {Description = "Task 3"}
        };

        dbContext.TodoTasks.AddRange(tasks);
        dbContext.SaveChanges();

        // Act
        var result = await sut.GetTodoTasks();

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeEmpty().And.HaveSameCount(tasks);
            result.Should().Equal(tasks, (actual, expected) => actual.Id == expected.Id);
            result.Should().Equal(tasks, (actual, expected) => actual.Description == expected.Description);
        }

        // Cleanup
        dbContext.TodoTasks.RemoveRange(tasks);
        dbContext.SaveChanges();
    }

    [Test]
    public async Task UpdateTodoTask_ShouldUpdateTodoTask_WhenTodoTaskExists()
    {
        // Arrange
        var dbContext = this.CreateDbContext;
        var sut = CreateSystemUnderTest(dbContext);

        var taskToUpdate = new TodoTask
        {
            Description = "Update me",
            DueDate = DateTime.Now.AddDays(5),
            IsComplete = false
        };

        dbContext.TodoTasks.Add(taskToUpdate);
        dbContext.SaveChanges();

        var updateTaskDto = new UpdateTodoTaskDto
        {
            Id = taskToUpdate.Id,
            IsComplete = true
        };

        // Act
        var result = await sut.UpdateTodoTask(updateTaskDto);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result!.Id.Should().Be(taskToUpdate.Id);
            result.Description.Should().Be(taskToUpdate.Description);
            result.DueDate.Should().Be(taskToUpdate.DueDate);
            result.IsComplete.Should().Be(updateTaskDto.IsComplete);
        }

        // Cleanup
        dbContext.TodoTasks.Remove(taskToUpdate);
        dbContext.SaveChanges();
    }

    [Test]
    public async Task UpdateTodoTask_ShouldThrowException_WhenTodoTaskNotFound()
    {
        // Arrange
        var dbContext = this.CreateDbContext;
        var sut = CreateSystemUnderTest(dbContext);

        var updateTaskDto = new UpdateTodoTaskDto
        {
            Id = 12,
            IsComplete = true
        };

        // Act
        var action = async () => await sut.UpdateTodoTask(updateTaskDto);

        // Assert
        await action.Should().ThrowAsync<Exception>();
    }

    private static TodoTaskService CreateSystemUnderTest(TodoDbContext dbContext) => new(dbContext);
}
