using Application.Common.ResultSupport;
using Shouldly;

namespace Application.UnitTests.Tests.Common.ResultSupport;

public class ErrorTests
{
    [Fact]
    public void None_ShouldHaveEmptyCodeAndDescription()
    {
        // Act & Assert
        Error.None.Code.ShouldBe(string.Empty);
        Error.None.Description.ShouldBe(string.Empty);
        Error.None.Type.ShouldBe(ErrorTypeEnum.Failure);
    }

    [Fact]
    public void NullValue_ShouldHaveCorrectCodeAndDescription()
    {
        // Act & Assert
        Error.NullValue.Code.ShouldBe("General.Null");
        Error.NullValue.Description.ShouldBe("Null value was provided");
        Error.NullValue.Type.ShouldBe(ErrorTypeEnum.Failure);
    }

    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        // Arrange
        string code = "Test.Code";
        string description = "Test description";
        ErrorTypeEnum type = ErrorTypeEnum.Validation;

        // Act
        var error = new Error(code, description, type);

        // Assert
        error.Code.ShouldBe(code);
        error.Description.ShouldBe(description);
        error.Type.ShouldBe(type);
    }

    [Fact]
    public void Failure_ShouldCreateErrorWithFailureType()
    {
        // Arrange
        string code = "Test.Failure";
        string description = "Test failure description";

        // Act
        var error = Error.Failure(code, description);

        // Assert
        error.Code.ShouldBe(code);
        error.Description.ShouldBe(description);
        error.Type.ShouldBe(ErrorTypeEnum.Failure);
    }

    [Fact]
    public void NotFound_ShouldCreateErrorWithNotFoundType()
    {
        // Arrange
        string code = "Test.NotFound";
        string description = "Test not found description";

        // Act
        var error = Error.NotFound(code, description);

        // Assert
        error.Code.ShouldBe(code);
        error.Description.ShouldBe(description);
        error.Type.ShouldBe(ErrorTypeEnum.NotFound);
    }

    [Fact]
    public void Problem_ShouldCreateErrorWithProblemType()
    {
        // Arrange
        string code = "Test.Problem";
        string description = "Test problem description";

        // Act
        var error = Error.Problem(code, description);

        // Assert
        error.Code.ShouldBe(code);
        error.Description.ShouldBe(description);
        error.Type.ShouldBe(ErrorTypeEnum.Problem);
    }

    [Fact]
    public void Conflict_ShouldCreateErrorWithConflictType()
    {
        // Arrange
        string code = "Test.Conflict";
        string description = "Test conflict description";

        // Act
        var error = Error.Conflict(code, description);

        // Assert
        error.Code.ShouldBe(code);
        error.Description.ShouldBe(description);
        error.Type.ShouldBe(ErrorTypeEnum.Conflict);
    }

    [Fact]
    public void Error_AsRecord_ShouldImplementValueEquality()
    {
        // Arrange
        var error1 = new Error("Code", "Description", ErrorTypeEnum.Failure);
        var error2 = new Error("Code", "Description", ErrorTypeEnum.Failure);
        var error3 = new Error("Different", "Different", ErrorTypeEnum.Failure);

        // Act & Assert
        error1.ShouldBe(error2);
        error1.ShouldNotBe(error3);
        (error1 == error2).ShouldBeTrue();
        (error1 != error3).ShouldBeTrue();
    }
}
