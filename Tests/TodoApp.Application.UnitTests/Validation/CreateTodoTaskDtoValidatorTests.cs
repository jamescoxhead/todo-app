using FluentValidation.TestHelper;
using TodoApp.Application.Dtos;
using TodoApp.Application.Validation;

namespace TodoApp.Application.UnitTests.Validation;

[TestFixture]
public class CreateTodoTaskDtoValidatorTests
{
    private CreateTodoTaskDtoValidator sut = new();

    [SetUp]
    public void TestCaseSetUp() => this.sut = new();

    [Test]
    public void Validate_ShouldValidateValidObject()
    {
        // Arrange
        var dto = new CreateTodoTaskDto
        {
            Description = "description",
            DueDate = DateTime.Now.AddDays(1),
        };

        // Act
        var result = this.sut.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [TestCase("")]
    [TestCase(null!)]
    [TestCase("                  ")]
    public void Validate_ShouldHaveErrorsForEmptyDescription(string description)
    {
        // Arrange
        var dto = new CreateTodoTaskDto
        {
            Description = description,
            DueDate = DateTime.Now.AddDays(1),
        };

        // Act
        var result = this.sut.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(d => d.Description);
    }

    [TestCase(751)]
    [TestCase(800)]
    [TestCase(8000)]
    public void Validate_ShouldHaveErrorForLongDescription(int length)
    {
        // Arrange
        var dto = new CreateTodoTaskDto
        {
            Description = new string('*', length),
            DueDate = DateTime.Now.AddDays(1),
        };

        // Act
        var result = this.sut.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(d => d.Description);
    }

    [Test]
    public void Validate_ShouldHaveErrorWhenDateInPast()
    {
        // Arrange
        var dto = new CreateTodoTaskDto
        {
            Description = "description",
            DueDate = DateTime.Now.AddDays(-5),
        };

        // Act
        var result = this.sut.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(d => d.DueDate);
    }
}
