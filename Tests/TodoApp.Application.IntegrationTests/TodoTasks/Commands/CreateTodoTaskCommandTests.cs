using AutoMapper;
using FluentAssertions.Execution;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Mapping;
using TodoApp.Application.TodoTasks.Commands;

namespace TodoApp.Application.IntegrationTests.TodoTasks.Commands;

[TestFixture]
public class CreateTodoTaskCommandTests : SqlLiteTestBase
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
        var action = () => new CreateTodoTaskCommandHandler(null!, this.mapper);

        action.Should().ThrowExactly<ArgumentNullException>().WithParameterName("dbContext");
    }

    [Test]
    public void Contructor_ShouldThrowNullReferenceException_WhenMapperIsNull()
    {
        var action = () => new CreateTodoTaskCommandHandler(this.CreateDbContext, null!);

        action.Should().ThrowExactly<ArgumentNullException>().WithParameterName("mapper");
    }

    [Test]
    public void Constructor_ShouldInstantiate_WithValidParameters()
    {
        var sut = this.CreateSystemUnderTest(this.CreateDbContext);

        sut.Should().NotBeNull();
    }

    [Test]
    public async Task CreateTodoTask_ShouldCreateNewTodoTask()
    {
        // Arrange
        var dbContext = this.CreateDbContext;
        var sut = this.CreateSystemUnderTest(dbContext);

        var newTask = new CreateTodoTaskCommand
        {
            Description = "Test task",
            DueDate = DateTime.Now.AddDays(1)
        };

        // Act
        var result = await sut.Handle(newTask, CancellationToken.None);

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

    private CreateTodoTaskCommandHandler CreateSystemUnderTest(ITodoDbContext dbContext) => new(dbContext, this.mapper);
}
