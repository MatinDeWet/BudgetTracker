using Application.Common.ResultSupport;
using Shouldly;

namespace Application.UnitTests.Tests.Common.ResultSupport;
public class OptionExtensionsTests
{
    [Fact]
    public void Match_WhenOptionHasValue_ShouldExecuteOnSome()
    {
        // Arrange
        var option = Option<string>.Some("test");
        string onSomeExecuted = string.Empty;
        string onNoneExecuted = string.Empty;

        // Act
        string result = option.Match(
            onSome: value => { onSomeExecuted = value; return $"Some: {value}"; },
            onNone: () => { onNoneExecuted = "executed"; return "None"; }
        );

        // Assert
        result.ShouldBe("Some: test");
        onSomeExecuted.ShouldBe("test");
        onNoneExecuted.ShouldBeEmpty();
    }

    [Fact]
    public void Match_WhenOptionHasNoValue_ShouldExecuteOnNone()
    {
        // Arrange
        var option = Option<string>.None();
        string onSomeExecuted = string.Empty;
        string onNoneExecuted = string.Empty;

        // Act
        string result = option.Match(
            onSome: value => { onSomeExecuted = value; return $"Some: {value}"; },
            onNone: () => { onNoneExecuted = "executed"; return "None"; }
        );

        // Assert
        result.ShouldBe("None");
        onSomeExecuted.ShouldBeEmpty();
        onNoneExecuted.ShouldBe("executed");
    }

    [Fact]
    public void ToResult_WithCustomError_WhenOptionHasValue_ShouldReturnSuccessResult()
    {
        // Arrange
        var option = Option<int>.Some(42);
        var error = Error.Failure("Custom.Error", "This is a custom error");

        // Act
        var result = option.ToResult(error);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();
        result.Value.ShouldBe(42);
        result.Error.ShouldBe(Error.None);
    }

    [Fact]
    public void ToResult_WithCustomError_WhenOptionHasNoValue_ShouldReturnFailureResult()
    {
        // Arrange
        var option = Option<int>.None();
        var error = Error.Failure("Custom.Error", "This is a custom error");

        // Act
        var result = option.ToResult(error);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(error);
        Should.Throw<InvalidOperationException>(() => _ = result.Value);
    }

    [Fact]
    public void ToResult_WithoutError_WhenOptionHasValue_ShouldReturnSuccessResult()
    {
        // Arrange
        var option = Option<string>.Some("test");

        // Act
        var result = option.ToResult();

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();
        result.Value.ShouldBe("test");
        result.Error.ShouldBe(Error.None);
    }

    [Fact]
    public void ToResult_WithoutError_WhenOptionHasNoValue_ShouldReturnFailureResultWithNullValueError()
    {
        // Arrange
        var option = Option<string>.None();

        // Act
        var result = option.ToResult();

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(Error.NullValue);
        Should.Throw<InvalidOperationException>(() => _ = result.Value);
    }
}
