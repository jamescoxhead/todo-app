using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Controllers;
using TodoApp.Application.Dtos;
using TodoApp.Application.Interfaces;

namespace TodoApp.Api.UnitTests.Controllers;

[TestFixture]
public class TodoTasksControllerTests
{
    private readonly Mock<ITodoTaskService> mockTodoTaskService = new();

    [SetUp]
    public void TestCaseSetUp() => this.mockTodoTaskService.Reset();

    [Test]
    public void Contructor_ShouldThrowNullReferenceException_WhenTodoTaskServiceIsNull()
    {
        var action = () => new TodoTasksController(null!);

        action.Should().ThrowExactly<ArgumentNullException>().WithParameterName("todoTaskService");
    }

    [Test]
    public void Constructor_ShouldInstantiate_WithValidParameters()
    {
        var sut = this.CreateSystemUnderTest();

        sut.Should().NotBeNull();
    }

    [Test]
    public async Task GetTodoTasks_ShouldReturnOkResult_WhenResultsFound()
    {
        // Arrange
        // TODO: Autofixture/Bogus
        var model = new List<TodoTaskDto>
        {
            new TodoTaskDto {Id = 1, Description = "Task 1", DueDate = DateTime.Now, IsComplete = false},
            new TodoTaskDto {Id = 2, Description = "Task 2", DueDate = DateTime.Now.AddDays(2), IsComplete = true},
            new TodoTaskDto {Id = 3, Description = "Task 3", DueDate = DateTime.Now.AddMonths(3), IsComplete = false}
        };

        this.mockTodoTaskService.Setup(m => m.GetTodoTasks().Result).Returns(model);

        var sut = this.CreateSystemUnderTest();

        // Act
        var result = await sut.GetTodoTasks();

        // Assert
        result.Should().NotBeNull().And.BeOfType<OkObjectResult>()
              .Which.Value.Should().BeAssignableTo<IEnumerable<TodoTaskDto>>()
              .Which.Should().Contain(model);
    }

    [Test]
    public async Task GetTodoTask_ShouldReturnOkResult_WhenResultFound()
    {
        // Arrange
        var model = new TodoTaskDto { Id = 1, Description = "Task 1", DueDate = DateTime.Now, IsComplete = false };

        this.mockTodoTaskService.Setup(m => m.GetTodoTaskById(model.Id).Result).Returns(model);
        var sut = this.CreateSystemUnderTest();

        // Act
        var result = await sut.GetTodoTask(model.Id);

        // Assert
        result.Should().NotBeNull().And.BeOfType<OkObjectResult>()
              .Which.Value.Should().BeOfType<TodoTaskDto>()
              .Which.Should().Be(model);
    }

    [Test]
    public async Task GetTodoTask_ShouldReturnNotFoundResult_IfResultNotFound()
    {
        // Arrange
        var model = new TodoTaskDto { Id = 1, Description = "Task 1", DueDate = DateTime.Now, IsComplete = false };

        this.mockTodoTaskService.Setup(m => m.GetTodoTaskById(model.Id).Result).Returns(model);
        var sut = this.CreateSystemUnderTest();

        // Act
        var result = await sut.GetTodoTask(2);

        // Assert
        result.Should().NotBeNull().And.BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task CreateTodoTask_ShouldReturnOkResult()
    {
        // Arrange
        var inputModel = new CreateTodoTaskDto { Description = "New task", DueDate = DateTime.Now.AddDays(7) };
        var returnModel = new TodoTaskDto { Description = inputModel.Description, DueDate = inputModel.DueDate, Id = 1, IsComplete = false };

        this.mockTodoTaskService.Setup(m => m.CreateTodoTask(inputModel).Result).Returns(returnModel);
        var sut = this.CreateSystemUnderTest();

        // Act
        var result = await sut.CreateTodoTask(inputModel);

        // Assert
        result.Should().NotBeNull().And.BeOfType<OkObjectResult>()
              .Which.Value.Should().BeOfType<TodoTaskDto>()
              .Which.Should().Be(returnModel);
    }

    [Test]
    public async Task UpdateTodoTask_ShouldReturnOkResult_WhenUpdated()
    {
        // Arrange
        var inputModel = new UpdateTodoTaskDto { Id = 123, IsComplete = true };
        var returnModel = new TodoTaskDto { Description = "Task 123", DueDate = null, Id = inputModel.Id, IsComplete = inputModel.IsComplete };

        this.mockTodoTaskService.Setup(m => m.UpdateTodoTask(inputModel).Result).Returns(returnModel);
        var sut = this.CreateSystemUnderTest();

        // Act
        var result = await sut.UpdateTodoTask(inputModel.Id, inputModel);

        // Assert
        result.Should().NotBeNull().And.BeOfType<OkObjectResult>()
              .Which.Value.Should().BeOfType<TodoTaskDto>()
              .Which.Should().Be(returnModel);
    }

    [Test]
    public async Task UpdateTodoTask_ShouldReturnBadRequestResult_IfIdMismatch()
    {
        // Arrange
        var inputModel = new UpdateTodoTaskDto { Id = 123, IsComplete = true };
        var returnModel = new TodoTaskDto { Description = "Task 123", DueDate = null, Id = 456, IsComplete = inputModel.IsComplete };

        this.mockTodoTaskService.Setup(m => m.UpdateTodoTask(inputModel).Result).Returns(returnModel);
        var sut = this.CreateSystemUnderTest();

        // Act
        var result = await sut.UpdateTodoTask(12, inputModel);

        // Assert
        result.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task UpdateTodoTask_ShouldReturnBadRequestResult_IfNotUpdated()
    {
        // Arrange
        var inputModel = new UpdateTodoTaskDto { Id = 123, IsComplete = true };
        var returnModel = new TodoTaskDto { Description = "Task 123", DueDate = null, Id = inputModel.Id, IsComplete = inputModel.IsComplete };

        this.mockTodoTaskService.Setup(m => m.UpdateTodoTask(inputModel).Result).Throws<Exception>();
        var sut = this.CreateSystemUnderTest();

        // Act
        var result = await sut.UpdateTodoTask(inputModel.Id, inputModel);

        // Assert
        result.Should().NotBeNull().And.BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task DeleteTodoTask_ShouldReturnNoContent_IfDeleted()
    {
        // Arrange
        this.mockTodoTaskService.Setup(m => m.DeleteTodoTask(It.IsAny<int>()));
        var sut = this.CreateSystemUnderTest();

        // Act
        var result = await sut.DeleteTodoTask(123);

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
    }

    [Test]
    public async Task DeleteTodoTask_ShouldReturnBadRequestResult_IfNotDeleted()
    {
        // Arrange
        this.mockTodoTaskService.Setup(m => m.DeleteTodoTask(It.IsAny<int>())).ThrowsAsync(new Exception());
        var sut = this.CreateSystemUnderTest();

        // Act
        var result = await sut.DeleteTodoTask(123);

        // Assert
        result.Should().NotBeNull().And.BeOfType<BadRequestResult>();
    }

    private TodoTasksController CreateSystemUnderTest() => new(this.mockTodoTaskService.Object);
}
