namespace Application.Common.ResultSupport;
/// <summary>
/// Provides extension methods for working with Result types.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Executes the appropriate function based on whether the result is successful or failed.
    /// </summary>
    /// <typeparam name="TOut">The type of the return value.</typeparam>
    /// <param name="result">The result to match on.</param>
    /// <param name="onSuccess">The function to execute if the result is successful.</param>
    /// <param name="onFailure">The function to execute if the result is a failure.</param>
    /// <returns>The result of the executed function.</returns>
    public static TOut Match<TOut>(
        this Result result,
        Func<TOut> onSuccess,
        Func<Result, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result);
    }

    /// <summary>
    /// Executes the appropriate function based on whether the result is successful or failed.
    /// </summary>
    /// <typeparam name="TIn">The type of the value in case of success.</typeparam>
    /// <typeparam name="TOut">The type of the return value.</typeparam>
    /// <param name="result">The result to match on.</param>
    /// <param name="onSuccess">The function to execute if the result is successful.</param>
    /// <param name="onFailure">The function to execute if the result is a failure.</param>
    /// <returns>The result of the executed function.</returns>
    public static TOut Match<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> onSuccess,
        Func<Result<TIn>, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
    }
}
