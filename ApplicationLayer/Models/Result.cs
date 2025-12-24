public class OkResult<T>
{
    public T? Data;
}

public class ErrResult<E>
{
    public required E Code;
    public object? Meta;
}

public class Result
{
    public static OkResult<T> Ok<T>(T? data)
    {
        return new OkResult<T> { Data = data };
    }

    public static ErrResult<E> Err<E>(E code, object? meta = null)
    {
        return new ErrResult<E> { Code = code, Meta = meta };
    }
}
