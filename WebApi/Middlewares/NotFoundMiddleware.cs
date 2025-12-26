public class NotFoundMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

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
