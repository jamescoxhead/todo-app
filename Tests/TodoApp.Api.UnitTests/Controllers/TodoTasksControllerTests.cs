using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoApp.Api.Controllers;
using TodoApp.Application.Exceptions;
using TodoApp.Application.TodoTasks.Commands;
using TodoApp.Application.TodoTasks.Dtos;
using TodoApp.Application.TodoTasks.Queries;
using TodoApp.Application.TodoTasks.Validators;

namespace TodoApp.Api.UnitTests.Controllers;

[TestFixture]
public class TodoTasksControllerTests
{
    private readonly Mock<ILogger<TodoTasksController>> mockLogger = new();
    private readonly CreateTodoTaskCommandValidator createValidator = new();
    private readonly Mock<IMediator> mockMediator = new();

    [SetUp]
    public void TestCaseSetUp() => this.mockMediator.Reset();

    [Test]
    public void Constructor_Should_ThrowArgumentNullException_WhenLoggerIsNull()
    {
        var action = () => new TodoTasksController(null!, this.createValidator, this.mockMediator.Object);

        action.Should().ThrowExactly<ArgumentNullException>().WithParameterName("logger");
    }

    [Test]
    public void Constructor_Should_ThrowArgumentNullException_WhenCreateValidatorIsNull()
    {
        var action = () => new TodoTasksController(this.mockLogger.Object, null!, this.mockMediator.Object);

        action.Should().ThrowExactly<ArgumentNullException>().WithParameterName("createTodoTaskValidator");
    }

    [Test]
    public void Constructor_Should_ThrowArgumentNullException_WhenMediatorIsNull()
    {
        var action = () => new TodoTasksController(this.mockLogger.Object, this.createValidator, null!);

        action.Should().ThrowExactly<ArgumentNullException>().WithParameterName("mediator");
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

        this.mockMediator.Setup(m => m.Send(It.IsAny<GetTodoTasksQuery>(), It.IsAny<CancellationToken>()).Result).Returns(model);

        var sut = this.CreateSystemUnderTest();

        // Act
        var result = await sut.GetTodoTasks();

        // Assert
        result.Should().NotBeNull().And.BeOfType<OkObjectResult>()
              .Which.Value.Should().BeAssignableTo<IEnumerable<TodoTaskDto>>()
              .Which.Should().Contain(model);
    }

    [Test]
    public async Task GetTodoTasks_ShouldReturnOkResult_WhenNoResultsFound()
    {
        // Arrange
        var model = Enumerable.Empty<TodoTaskDto>();

        this.mockMediator.Setup(m => m.Send(It.IsAny<GetTodoTasksQuery>(), It.IsAny<CancellationToken>()).Result).Returns(model);

        var sut = this.CreateSystemUnderTest();

        // Act
        var result = await sut.GetTodoTasks();

        // Assert
        result.Should().NotBeNull().And.BeOfType<OkObjectResult>()
              .Which.Value.Should().BeAssignableTo<IEnumerable<TodoTaskDto>>()
              .Which.Should().BeEmpty();
    }

    [Test]
    public async Task GetTodoTask_ShouldReturnOkResult_WhenResultFound()
    {
        // Arrange
        var model = new TodoTaskDto { Id = 1, Description = "Task 1", DueDate = DateTime.Now, IsComplete = false };

        this.mockMediator.Setup(m => m.Send(It.IsAny<GetTodoTaskQuery>(), It.IsAny<CancellationToken>()).Result).Returns(model);
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

        this.mockMediator.Setup(m => m.Send(new GetTodoTaskQuery { TodoTaskId = model.Id }, It.IsAny<CancellationToken>()).Result).Returns(model);
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
        var inputModel = new CreateTodoTaskCommand { Description = "New task", DueDate = DateTime.Now.AddDays(7) };
        var returnModel = new TodoTaskDto { Description = inputModel.Description, DueDate = inputModel.DueDate, Id = 1, IsComplete = false };

        this.mockMediator.Setup(m => m.Send(It.IsAny<CreateTodoTaskCommand>(), It.IsAny<CancellationToken>()).Result).Returns(returnModel);
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
        var inputModel = new UpdateTodoTaskCommand { TodoTaskId = 123, IsComplete = true };
        var returnModel = new TodoTaskDto { Description = "Task 123", DueDate = null, Id = inputModel.TodoTaskId, IsComplete = inputModel.IsComplete };

        this.mockMediator.Setup(m => m.Send(inputModel, It.IsAny<CancellationToken>()).Result).Returns(returnModel);
        var sut = this.CreateSystemUnderTest();

        // Act
        var result = await sut.UpdateTodoTask(inputModel.TodoTaskId, inputModel);

        // Assert
        result.Should().NotBeNull().And.BeOfType<OkObjectResult>()
              .Which.Value.Should().BeOfType<TodoTaskDto>()
              .Which.Should().Be(returnModel);
    }

    [Test]
    public async Task UpdateTodoTask_ShouldReturnBadRequestResult_IfIdMismatch()
    {
        // Arrange
        var inputModel = new UpdateTodoTaskCommand { TodoTaskId = 123, IsComplete = true };
        var returnModel = new TodoTaskDto { Description = "Task 123", DueDate = null, Id = 456, IsComplete = inputModel.IsComplete };

        this.mockMediator.Setup(m => m.Send(It.IsAny<UpdateTodoTaskCommand>(), It.IsAny<CancellationToken>()).Result).Returns(returnModel);
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
        var inputModel = new UpdateTodoTaskCommand { TodoTaskId = 123, IsComplete = true };
        var returnModel = new TodoTaskDto { Description = "Task 123", DueDate = null, Id = inputModel.TodoTaskId, IsComplete = inputModel.IsComplete };

        this.mockMediator.Setup(m => m.Send(It.IsAny<UpdateTodoTaskCommand>(), It.IsAny<CancellationToken>()).Result).Throws<Exception>();
        var sut = this.CreateSystemUnderTest();

        // Act
        var result = await sut.UpdateTodoTask(inputModel.TodoTaskId, inputModel);

        // Assert
        result.Should().NotBeNull().And.BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task DeleteTodoTask_ShouldReturnNoContent_IfDeleted()
    {
        // Arrange
        this.mockMediator.Setup(m => m.Send(It.IsAny<DeleteTodoTaskCommand>(), It.IsAny<CancellationToken>()));
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
        this.mockMediator.Setup(m => m.Send(It.IsAny<DeleteTodoTaskCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(new NotFoundException("TodoTask", 123));
        var sut = this.CreateSystemUnderTest();

        // Act
        var result = await sut.DeleteTodoTask(123);

        // Assert
        result.Should().NotBeNull().And.BeOfType<NotFoundResult>();
    }

    private TodoTasksController CreateSystemUnderTest() => new(this.mockLogger.Object, this.createValidator, this.mockMediator.Object);
}
