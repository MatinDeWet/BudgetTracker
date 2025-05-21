using Application.Common.ResultSupport;
using Shouldly;

namespace Application.UnitTests.Tests.Common.ResultSupport;
public class OptionTests
{
    [Fact]
    public void Some_WithValue_ShouldCreateOptionWithValue()
    {
        // Arrange
        string value = "test value";

        // Act
        var option = Option<string>.Some(value);

        // Assert
        option.HasValue.ShouldBeTrue();
        option.HasNoValue.ShouldBeFalse();
        option.Value.ShouldBe(value);
    }

    [Fact]
    public void Some_WithNullValue_ShouldCreateNoneOption()
    {
        // Act
        var option = Option<string>.Some(null);

        // Assert
        option.HasValue.ShouldBeFalse();
        option.HasNoValue.ShouldBeTrue();
        Should.Throw<InvalidOperationException>(() => _ = option.Value)
            .Message.ShouldBe("The value of an empty option cannot be accessed.");
    }

    [Fact]
    public void None_ShouldCreateOptionWithNoValue()
    {
        // Act
        var option = Option<string>.None();

        // Assert
        option.HasValue.ShouldBeFalse();
        option.HasNoValue.ShouldBeTrue();
        Should.Throw<InvalidOperationException>(() => _ = option.Value)
            .Message.ShouldBe("The value of an empty option cannot be accessed.");
    }

    [Fact]
    public void ImplicitConversion_FromNonNullValue_ShouldCreateSomeOption()
    {
        // Arrange
        string value = "test value";

        // Act
        Option<string> option = value;

        // Assert
        option.HasValue.ShouldBeTrue();
        option.HasNoValue.ShouldBeFalse();
        option.Value.ShouldBe(value);
    }

    [Fact]
    public void ImplicitConversion_FromNullValue_ShouldCreateNoneOption()
    {
        // Arrange
        string? value = null;

        // Act
        Option<string> option = value;

        // Assert
        option.HasValue.ShouldBeFalse();
        option.HasNoValue.ShouldBeTrue();
        Should.Throw<InvalidOperationException>(() => _ = option.Value);
    }

    [Fact]
    public void Value_WhenOptionHasValue_ShouldReturnValue()
    {
        // Arrange
        int value = 42;
        var option = Option<int>.Some(value);

        // Act
        int result = option.Value;

        // Assert
        result.ShouldBe(value);
    }

    [Fact]
    public void Value_WhenOptionHasNoValue_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var option = Option<int>.None();

        // Act & Assert
        InvalidOperationException exception = Should.Throw<InvalidOperationException>(() => _ = option.Value);
        exception.Message.ShouldBe("The value of an empty option cannot be accessed.");
    }

    [Fact]
    public void Option_WithReferenceType_ShouldHandleCorrectly()
    {
        // Arrange
        var obj = new TestClass { Property = "test" };

        // Act
        var option = Option<TestClass>.Some(obj);

        // Assert
        option.HasValue.ShouldBeTrue();
        option.Value.Property.ShouldBe("test");
    }

    [Fact]
    public void Option_WithValueType_ShouldHandleCorrectly()
    {
        // Arrange
        var value = new DateTime(2025, 5, 21);

        // Act
        var option = Option<DateTime>.Some(value);

        // Assert
        option.HasValue.ShouldBeTrue();
        option.Value.ShouldBe(value);
    }

    public sealed class TestClass
    {
        public string? Property { get; set; }
    }
}
