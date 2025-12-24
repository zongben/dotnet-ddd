public record RegisterRequest
{
    public required string Account { get; init; }
    public required string Password { get; init; }
    public required string UserName { get; init; }
}
