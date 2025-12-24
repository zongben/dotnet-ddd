public record LoginRequest
{
    public required string Account { get; init; }
    public required string Password { get; init; }
}
