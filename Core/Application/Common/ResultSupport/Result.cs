using System.Diagnostics.CodeAnalysis;

namespace Application.Common.ResultSupport;
/// <summary>
/// Represents the outcome of an operation that can succeed or fail.
/// </summary>
public class Result
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    /// <param name="isSuccess">Indicates whether the operation was successful.</param>
    /// <param name="error">The error if the operation failed, or <see cref="Error.None"/> if successful.</param>
    /// <exception cref="ArgumentException">Thrown when the combination of isSuccess and error is invalid.</exception>
    public Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the operation failed.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the error if the operation failed, or <see cref="Error.None"/> if successful.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// Creates a result representing a successful operation.
    /// </summary>
    /// <returns>A successful result.</returns>
    public static Result Success() => new(true, Error.None);

    /// <summary>
    /// Creates a result representing a successful operation with a value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value of the successful operation.</param>
    /// <returns>A successful result containing the specified value.</returns>
    public static Result<TValue> Success<TValue>(TValue value) =>
        new(value, true, Error.None);

    /// <summary>
    /// Creates a result representing a failed operation.
    /// </summary>
    /// <param name="error">The error that caused the operation to fail.</param>
    /// <returns>A failure result containing the specified error.</returns>
    public static Result Failure(Error error) => new(false, error);

    /// <summary>
    /// Creates a result representing a failed operation with a value type.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="error">The error that caused the operation to fail.</param>
    /// <returns>A failure result of the specified type containing the error.</returns>
    public static Result<TValue> Failure<TValue>(Error error) =>
        new(default, false, error);
}

/// <summary>
/// Represents the outcome of an operation that can succeed with a value or fail with an error.
/// </summary>
/// <typeparam name="TValue">The type of the value in case of success.</typeparam>
public class Result<TValue> : Result
{
    private readonly TValue? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TValue}"/> class.
    /// </summary>
    /// <param name="value">The value if the operation was successful.</param>
    /// <param name="isSuccess">Indicates whether the operation was successful.</param>
    /// <param name="error">The error if the operation failed, or <see cref="Error.None"/> if successful.</param>
    public Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    /// <summary>
    /// Gets the value of the successful operation.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when attempting to access the value of a failed operation.</exception>
    [NotNull]
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can't be accessed.");

    /// <summary>
    /// Implicitly converts a value to a successful result, or to a failure result if the value is null.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);

    /// <summary>
    /// Creates a result representing a validation failure.
    /// </summary>
    /// <param name="error">The validation error that caused the operation to fail.</param>
    /// <returns>A failure result containing the specified validation error.</returns>
    public static Result<TValue> ValidationFailure(Error error) =>
        new(default, false, error);
}
