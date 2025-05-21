using Application.Common.ResultSupport;
using Shouldly;

namespace Application.UnitTests.Tests.Common.ResultSupport;
public class ResultExtensionsTests
{
    [Fact]
    public void Match_WithoutValue_WhenResultIsSuccess_ShouldExecuteOnSuccess()
    {
        // Arrange
        var result = Result.Success();
        string onSuccessExecuted = string.Empty;
        string onFailureExecuted = string.Empty;

        // Act
        string matchResult = result.Match(
            onSuccess: () => { onSuccessExecuted = "executed"; return "Success"; },
            onFailure: r => { onFailureExecuted = r.Error.Description; return "Failure"; }
        );

        // Assert
        matchResult.ShouldBe("Success");
        onSuccessExecuted.ShouldBe("executed");
        onFailureExecuted.ShouldBeEmpty();
    }

    [Fact]
    public void Match_WithoutValue_WhenResultIsFailure_ShouldExecuteOnFailure()
    {
        // Arrange
        var error = Error.Failure("Test.Error", "Test error description");
        var result = Result.Failure(error);
        string onSuccessExecuted = string.Empty;
        string onFailureExecuted = string.Empty;

        // Act
        string matchResult = result.Match(
            onSuccess: () => { onSuccessExecuted = "executed"; return "Success"; },
            onFailure: r => { onFailureExecuted = r.Error.Description; return "Failure"; }
        );

        // Assert
        matchResult.ShouldBe("Failure");
        onSuccessExecuted.ShouldBeEmpty();
        onFailureExecuted.ShouldBe("Test error description");
    }

    [Fact]
    public void Match_WithValue_WhenResultIsSuccess_ShouldExecuteOnSuccess()
    {
        // Arrange
        var result = Result.Success(42);
        int onSuccessValue = 0;
        string onFailureExecuted = string.Empty;

        // Act
        string matchResult = result.Match(
            onSuccess: value => { onSuccessValue = value; return $"Success: {value}"; },
            onFailure: r => { onFailureExecuted = r.Error.Description; return "Failure"; }
        );

        // Assert
        matchResult.ShouldBe("Success: 42");
        onSuccessValue.ShouldBe(42);
        onFailureExecuted.ShouldBeEmpty();
    }

    [Fact]
    public void Match_WithValue_WhenResultIsFailure_ShouldExecuteOnFailure()
    {
        // Arrange
        var error = Error.Failure("Test.Error", "Test error description");
        var result = Result.Failure<int>(error);
        int onSuccessValue = 0;
        string onFailureExecuted = string.Empty;

        // Act
        string matchResult = result.Match(
            onSuccess: value => { onSuccessValue = value; return $"Success: {value}"; },
            onFailure: r => { onFailureExecuted = r.Error.Description; return "Failure"; }
        );

        // Assert
        matchResult.ShouldBe("Failure");
        onSuccessValue.ShouldBe(0); // Unchanged
        onFailureExecuted.ShouldBe("Test error description");
    }

    [Fact]
    public void Match_WithValue_ShouldReturnCorrectType()
    {
        // Arrange
        var successResult = Result.Success("test");
        var failureResult = Result.Failure<string>(Error.NullValue);

        // Act
        bool successMatch = successResult.Match(
            onSuccess: _ => true,
            onFailure: _ => false
        );

        bool failureMatch = failureResult.Match(
            onSuccess: _ => true,
            onFailure: _ => false
        );

        // Assert
        successMatch.ShouldBeTrue();
        failureMatch.ShouldBeFalse();
    }

    [Fact]
    public void Match_WithoutValue_ShouldReturnCorrectType()
    {
        // Arrange
        var successResult = Result.Success();
        var failureResult = Result.Failure(Error.NullValue);

        // Act
        bool successMatch = successResult.Match(
            onSuccess: () => true,
            onFailure: _ => false
        );

        bool failureMatch = failureResult.Match(
            onSuccess: () => true,
            onFailure: _ => false
        );

        // Assert
        successMatch.ShouldBeTrue();
        failureMatch.ShouldBeFalse();
    }
}
