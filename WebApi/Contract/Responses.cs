public class OkResponse<T>
{
    public T? data { get; }

    public OkResponse(OkResult<T> ok)
    {
        data = ok.Data;
    }
}

public class ErrResponse
{
    public string code { get; }
    public object? meta { get; }

    private ErrResponse(string code)
    {
        this.code = code;
    }

    public ErrResponse(ErrResult<ERROR_CODES> err)
    {
        code = Enum.GetName(typeof(ERROR_CODES), err.Code) ?? "UNKNOW";
        meta = err.Meta;
    }

    public static ErrResponse InvalidToken()
    {
        return new ErrResponse("INVALID_TOKEN");
    }

    public static ErrResponse InternalServerError()
    {
        return new ErrResponse("INTERNAL_SERVER_ERROR");
    }

    public static ErrResponse NotFound()
    {
        return new ErrResponse("NOT_FOUND");
    }

    public static ErrResponse Forbidden()
    {
        return new ErrResponse("FORBIDDEN");
    }

    public static ErrResponse TokenExpired()
    {
        return new ErrResponse("TOKEN_EXPIRED");
    }

    public static ErrResponse MissingToken()
    {
        return new ErrResponse("MISSING_TOKEN");
    }
}
