namespace TraininApp.Application.Common;

public sealed class Result
{
    private Result(Error? error) => Error = error;
    public Error? Error { get; }
    public bool IsSuccess => Error is null;
    public static Result Success() => new(null);
    public static Result Failure(string code, string message) => new(new Error(code, message));
}

