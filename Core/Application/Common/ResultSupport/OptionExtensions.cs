namespace Application.Common.ResultSupport;
/// <summary>
/// Provides extension methods for working with Option types.
/// </summary>
public static class OptionExtensions
{
    /// <summary>
    /// Executes the appropriate function based on whether the option has a value.
    /// </summary>
    public static TOut Match<TIn, TOut>(
        this Option<TIn> option,
        Func<TIn, TOut> onSome,
        Func<TOut> onNone)
    {
        return option.HasValue ? onSome(option.Value) : onNone();
    }

    /// <summary>
    /// Converts an option to a result.
    /// </summary>
    public static Result<TValue> ToResult<TValue>(
        this Option<TValue> option,
        Error error)
    {
        return option.HasValue
            ? Result.Success(option.Value)
            : Result.Failure<TValue>(error);
    }

    /// <summary>
    /// Converts an option to a result with a default error for none case.
    /// </summary>
    public static Result<TValue> ToResult<TValue>(
        this Option<TValue> option)
    {
        return option.ToResult(Error.NullValue);
    }
}
