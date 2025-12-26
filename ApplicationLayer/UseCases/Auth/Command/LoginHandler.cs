using MediatR;
using OneOf;

public record LoginResult
{
    public required string Token { get; init; }
}

public record LoginCommand : IRequest<OneOf<OkResult<LoginResult>, ErrResult<ERROR_CODES>>>
{
    public required string Account { get; init; }
    public required string Password { get; init; }
}

public class LoginHandler(
    IUserRepository userRepository,
    ICryptService cryptService,
    ITokenService tokenService
    )
        : IRequestHandler<LoginCommand, OneOf<OkResult<LoginResult>, ErrResult<ERROR_CODES>>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICryptService _cryptService = cryptService;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<OneOf<OkResult<LoginResult>, ErrResult<ERROR_CODES>>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetUserByAccount(request.Account);
        if (user is null)
        {
            return Result.Err(ERROR_CODES.LOGIN_FAILED);
        }

        var isPwdValid = await _cryptService.Verify(request.Password, user.HashedPassword);
        if (!isPwdValid)
        {
            return Result.Err(ERROR_CODES.LOGIN_FAILED);
        }

        var token = _tokenService.GenerateToken(new JwtPayload(user.Id.Value, user.UserName));
        return Result.Ok(new LoginResult { Token = token });
    }
}
