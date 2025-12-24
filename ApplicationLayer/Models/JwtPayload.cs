public class JwtPayload
{
    public string UserId { get; }
    public string UserName { get; }

    public JwtPayload(string userId, string username)
    {
        UserId = userId;
        UserName = username;
    }
}
