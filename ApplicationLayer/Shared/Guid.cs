namespace ApplicationLayer.Shared;

public static class GID
{
    public static string Create()
    {
        return Guid.CreateVersion7().ToString();
    }
}
