public class UserId : ValueObject
{
    public string Value { get; }

    private UserId(string value)
    {
        Value = value;
    }

    public static UserId Create(string value)
    {
        return new UserId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public class User : AggregateRoot<UserId>
{
    public string Account { get; private set; }
    public string UserName { get; private set; }
    public string HashedPassword { get; private set; }

    private User(UserId userId, string account, string hashedPassword, string userName)
        : base(userId)
    {
        Account = account;
        HashedPassword = hashedPassword;
        UserName = userName;
    }

    public static User Create(UserId userId, string account, string hashedPassword, string userName)
    {
        return new User(userId, account, hashedPassword, userName);
    }
}
