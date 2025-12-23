using DomainLayer.User;

namespace ApplicationLayer.Persistence;

public interface IUserRepository
{
    Task<User?> GetUser(UserId userId);
    Task<User?> GetUserByAccount(string account);
    Task<User> Create(User user);
    Task<User> Save(User user);
}
