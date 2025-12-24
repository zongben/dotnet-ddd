public interface ITokenService
{
    string GenerateToken(JwtPayload payload);
}
