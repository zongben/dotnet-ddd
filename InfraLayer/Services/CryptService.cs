using System.Security.Cryptography;

public class CryptService : ICryptService
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100_000;

    public Task<string> Hash(string plain)
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[SaltSize];
        rng.GetBytes(salt);

        var key = Rfc2898DeriveBytes.Pbkdf2(
            plain,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            KeySize
        );

        var hashedBytes = new byte[SaltSize + KeySize];
        Buffer.BlockCopy(salt, 0, hashedBytes, 0, SaltSize);
        Buffer.BlockCopy(key, 0, hashedBytes, SaltSize, KeySize);

        return Task.FromResult(Convert.ToBase64String(hashedBytes));
    }

    public Task<bool> Verify(string plain, string hash)
    {
        var hashBytes = Convert.FromBase64String(hash);

        var salt = new byte[SaltSize];
        Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);

        var key = new byte[KeySize];
        Buffer.BlockCopy(hashBytes, SaltSize, key, 0, KeySize);

        var keyToCheck = Rfc2898DeriveBytes.Pbkdf2(
            plain,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            KeySize
        );

        var verified = CryptographicOperations.FixedTimeEquals(key, keyToCheck);
        return Task.FromResult(verified);
    }
}
