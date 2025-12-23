namespace ApplicationLayer.Ports;

public interface ICryptService
{
    Task<string> Hash(string plain);
    Task<bool> Verify(string plain, string hash);
}
