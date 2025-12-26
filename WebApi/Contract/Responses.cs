public class OkResponse<T>(OkResult<T> ok)
{
    public T? Data { get; } = ok.Data;
}

public class ErrResponse
{
    public string Code { get; }
    public string? Message { get; }
    public object? Meta { get; }

    private ErrResponse(string code, string? message = "")
    {
        Code = code;
        Message = message;
    }

    public ErrResponse(ErrResult<ERROR_CODES> err, string? message = "")
    {
        Code = Enum.GetName(typeof(ERROR_CODES), err.Code) ?? "UNKNOW";
        Message = message;
        Meta = err.Meta;
    }

    public static ErrResponse InvalidToken()
    {
        return new ErrResponse("INVALID_TOKEN", "token is not valid");
    }

    public static ErrResponse InternalServerError()
    {
        return new ErrResponse("INTERNAL_SERVER_ERROR", "internal server error");
    }

    public static ErrResponse NotFound()
    {
        return new ErrResponse("NOT_FOUND", "not found");
    }

    public static ErrResponse Forbidden()
    {
        return new ErrResponse("FORBIDDEN", "forbidden");
    }

    public static ErrResponse TokenExpired()
    {
        return new ErrResponse("TOKEN_EXPIRED", "token is expired");
    }

    public static ErrResponse MissingToken()
    {
        return new ErrResponse("MISSING_TOKEN", "token is missing");
    }

    public static ErrResponse RequestInvalid(string message)
    {
        return new ErrResponse("REQUEST_INVALID", message);
    }
}
