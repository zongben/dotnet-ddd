using ApplicationLayer.Models;
using ApplicationLayer.Persistence;
using ApplicationLayer.Ports;
using ApplicationLayer.Shared;
using DomainLayer.User;
using MediatR;
using OneOf;

namespace ApplicationLayer.UseCases.Auth.Command.Register;

public record RegisterCommand : IRequest<OneOf<OkResult<RegisterResult>, ErrResult<ERROR_CODES>>>
{
    public required string Account { get; init; }
    public required string Password { get; init; }
    public required string UserName { get; init; }
}

public record RegisterResult
{
    public required string Account { get; init; }
    public required string UserName { get; init; }
}

public class RegisterHandler
    : IRequestHandler<RegisterCommand, OneOf<OkResult<RegisterResult>, ErrResult<ERROR_CODES>>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICryptService _cryptService;

    public RegisterHandler(IUserRepository userRepository, ICryptService cryptService)
    {
        _userRepository = userRepository;
        _cryptService = cryptService;
    }

    public async Task<OneOf<OkResult<RegisterResult>, ErrResult<ERROR_CODES>>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken
    )
    {
        var isUserExists = await _userRepository.GetUserByAccount(request.Account) is not null;
        if (isUserExists)
        {
            return Result.Err(ERROR_CODES.ACCOUNT_IS_USED);
        }

        var user = User.Create(
            UserId.Create(GID.Create()),
            request.Account,
            await _cryptService.Hash(request.Password),
            request.UserName
        );

        await _userRepository.Create(user);

        return Result.Ok(new RegisterResult { Account = user.Account, UserName = user.UserName });
    }
}
