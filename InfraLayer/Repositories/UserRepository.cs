using Microsoft.EntityFrameworkCore;

public class UserRepository(AppDbContext db) : IUserRepository
{
    private AppDbContext _db { get; } = db;

    public async Task<User> Create(User user)
    {
        await _db.Users.AddAsync(
            new UserEntity
            {
                Id = user.Id.Value,
                Account = user.Account,
                Password = user.HashedPassword,
                UserName = user.UserName,
                CreatedAt = DateTime.UtcNow,
            }
        );

        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetUser(UserId userId)
    {
        var user = await _db.Users.Where(x => x.Id == userId.Value).FirstOrDefaultAsync();
        if (user is null)
        {
            return null;
        }
        return User.Create(UserId.Create(user.Id), user.Account, user.Password, user.UserName);
    }

    public async Task<User?> GetUserByAccount(string account)
    {
        var user = await _db.Users.Where(x => x.Account == account).FirstOrDefaultAsync();
        if (user is null)
        {
            return null;
        }
        return User.Create(UserId.Create(user.Id), user.Account, user.Password, user.UserName);
    }

    public async Task<User> Save(User user)
    {
        var entity = await _db.Users.Where(x => x.Id == user.Id.Value).FirstOrDefaultAsync();
        if (entity is null)
        {
            throw new Exception($"User Id {user.Id.Value} is not found");
        }
        entity.Account = user.Account;
        entity.Password = user.HashedPassword;
        entity.UserName = user.UserName;
        await _db.SaveChangesAsync();

        return user;
    }
}
