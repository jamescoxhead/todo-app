using AutoMapper;
using FluentAssertions.Execution;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Mapping;
using TodoApp.Application.TodoTasks.Queries;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.IntegrationTests.TodoTasks.Queries;

[TestFixture]
public class GetTodoTasksQueryTests : SqlLiteTestBase
{
    private IMapper mapper;

    [OneTimeSetUp]
    public void TestFixtureSetUp()
    {
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<TodoTaskMappingProfile>());
        this.mapper = mapperConfig.CreateMapper();
    }

    [Test]
    public async Task GetTodoTasks_ShouldReturnTodoTasks()
    {
        // Arrange
        var dbContext = this.CreateDbContext;
        var sut = this.CreateSystemUnderTest(dbContext);

        var tasks = new List<TodoTask>
        {
            new TodoTask {Description = "Task 1"},
            new TodoTask {Description = "Task 2"},
            new TodoTask {Description = "Task 3"}
        };

        dbContext.TodoTasks.AddRange(tasks);
        dbContext.SaveChanges();

        // Act
        var result = await sut.Handle(new GetTodoTasksQuery(), CancellationToken.None);

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

    private GetTodoTasksQueryHandler CreateSystemUnderTest(ITodoDbContext dbContext) => new(dbContext, this.mapper);
}
