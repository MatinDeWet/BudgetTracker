using Application.Common.ResultSupport;
using Shouldly;

namespace Application.UnitTests.Tests.Common.ResultSupport;
public class ResultTests
{
    [Fact]
    public void Constructor_WhenSuccessWithNoneError_ShouldCreateSuccessResult()
    {
        // Act
        var result = new Result(true, Error.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();
        result.Error.ShouldBe(Error.None);
    }

    [Fact]
    public void Constructor_WhenFailureWithCustomError_ShouldCreateFailureResult()
    {
        // Arrange
        var error = Error.Failure("Test.Error", "Test error description");

        // Act
        var result = new Result(false, error);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(error);
    }

    [Fact]
    public void Constructor_WhenSuccessWithNonNoneError_ShouldThrowArgumentException()
    {
        // Arrange
        var error = Error.Failure("Test.Error", "Test error description");

        // Act & Assert
        ArgumentException exception = Should.Throw<ArgumentException>(() => new Result(true, error));
        exception.ParamName.ShouldBe("error");
    }

    [Fact]
    public void Constructor_WhenFailureWithNoneError_ShouldThrowArgumentException()
    {
        // Act
        ArgumentException exception = Should.Throw<ArgumentException>(() => new Result(false, Error.None));

        // Assert
        exception.ParamName.ShouldBe("error");
    }

    [Fact]
    public void Success_ShouldCreateSuccessResult()
    {
        // Act
        var result = Result.Success();

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();
        result.Error.ShouldBe(Error.None);
    }

    [Fact]
    public void Failure_ShouldCreateFailureResult()
    {
        // Arrange
        var error = Error.Failure("Test.Error", "Test error description");

        // Act
        var result = Result.Failure(error);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(error);
    }

    [Fact]
    public void GenericSuccess_ShouldCreateSuccessResultWithValue()
    {
        // Arrange
        string value = "test value";

        // Act
        var result = Result.Success(value);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();
        result.Error.ShouldBe(Error.None);
        result.Value.ShouldBe(value);
    }

    [Fact]
    public void GenericFailure_ShouldCreateFailureResultWithoutValue()
    {
        // Arrange
        var error = Error.Failure("Test.Error", "Test error description");

        // Act
        var result = Result.Failure<string>(error);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(error);

        // Accessing Value property should throw
        Should.Throw<InvalidOperationException>(() => result.Value)
            .Message.ShouldContain("The value of a failure result can't be accessed");
    }

    [Fact]
    public void ImplicitConversion_FromNonNullValue_ShouldCreateSuccessResult()
    {
        // Arrange
        string value = "test value";

        // Act
        Result<string> result = value;

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(value);
    }

    [Fact]
    public void ImplicitConversion_FromNullValue_ShouldCreateFailureResult()
    {
        // Arrange
        string? value = null;

        // Act
        Result<string> result = value;

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(Error.NullValue);
    }

    [Fact]
    public void ValidationFailure_ShouldCreateFailureResult()
    {
        // Arrange
        var error = Error.Failure("Validation.Error", "Validation error description");

        // Act
        var result = Result<int>.ValidationFailure(error);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(error);
    }
}
