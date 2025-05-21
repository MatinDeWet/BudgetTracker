using System.Diagnostics.CodeAnalysis;

namespace Application.Common.ResultSupport;
/// <summary>
/// Represents an optional value that may or may not be present.
/// </summary>
public class Option<TValue>
{
    private readonly TValue? _value;

    private Option(TValue? value, bool hasValue)
    {
        _value = value;
        HasValue = hasValue;
    }

    /// <summary>
    /// Indicates whether the option has a value.
    /// </summary>
    public bool HasValue { get; }

    /// <summary>
    /// Indicates whether the option has no value.
    /// </summary>
    public bool HasNoValue => !HasValue;

    /// <summary>
    /// Gets the value if present, otherwise throws an exception.
    /// </summary>
    [NotNull]
    public TValue Value => HasValue
        ? _value!
        : throw new InvalidOperationException("The value of an empty option cannot be accessed.");

    /// <summary>
    /// Creates an option with a value.
    /// </summary>
    public static Option<TValue> Some(TValue? value) =>
        value is not null
            ? new Option<TValue>(value, true)
            : None();

    /// <summary>
    /// Creates an option with no value.
    /// </summary>
    public static Option<TValue> None() => new(default, false);

    /// <summary>
    /// Implicitly converts a value to an option.
    /// </summary>
    public static implicit operator Option<TValue>(TValue? value) =>
        value is not null ? Some(value) : None();
}
