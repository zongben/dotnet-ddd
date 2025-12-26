public class JwtPayload(string userId, string username)
{
    public string UserId { get; } = userId;
    public string UserName { get; } = username;
}
