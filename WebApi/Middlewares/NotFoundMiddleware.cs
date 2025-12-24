public class NotFoundMiddleware
{
    private readonly RequestDelegate _next;

    public NotFoundMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        await _next(context);

        if (context.Response.HasStarted)
        {
            return;
        }

        if (context.Response.StatusCode == StatusCodes.Status404NotFound)
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(ErrResponse.NotFound());
            return;
        }
    }
}
