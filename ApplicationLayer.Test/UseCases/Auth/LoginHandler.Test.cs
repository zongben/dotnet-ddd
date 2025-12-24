using Moq;

public class LoginHandlerTest
{
    [Fact]
    public async Task UserNotFoundShouldFailed()
    {
        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(m => m.GetUserByAccount(It.IsAny<string>())).ReturnsAsync((User?)null);

        var cryptService = new Mock<ICryptService>();
        var tokenService = new Mock<ITokenService>();

        var loginHandler = new LoginHandler(
            userRepository.Object,
            cryptService.Object,
            tokenService.Object
        );

        var command = new LoginCommand { Account = "Account", Password = "Password" };

        var result = await loginHandler.Handle(command, CancellationToken.None);

        Assert.False(result.IsT0);
        Assert.True(result.IsT1);
        Assert.Equal(ERROR_CODES.LOGIN_FAILED, result.AsT1.Code);
    }

    [Fact]
    public async Task PasswordInvalidShouldFailed()
    {
        var user = User.Create(UserId.Create(""), "account", "hashedPwd", "username");

        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(m => m.GetUserByAccount(It.IsAny<string>())).ReturnsAsync(user);

        var cryptService = new Mock<ICryptService>();
        cryptService
            .Setup(m => m.Verify(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        var tokenService = new Mock<ITokenService>();

        var loginHandler = new LoginHandler(
            userRepository.Object,
            cryptService.Object,
            tokenService.Object
        );

        var command = new LoginCommand
        {
            Account = It.IsAny<string>(),
            Password = "different_password",
        };

        var result = await loginHandler.Handle(command, CancellationToken.None);

        Assert.False(result.IsT0);
        Assert.True(result.IsT1);
        Assert.Equal(ERROR_CODES.LOGIN_FAILED, result.AsT1.Code);
    }

    [Fact]
    public async Task Success()
    {
        var user = User.Create(UserId.Create(""), "account", "hashedPwd", "username");

        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(m => m.GetUserByAccount(It.IsAny<string>())).ReturnsAsync(user);

        var cryptService = new Mock<ICryptService>();
        cryptService
            .Setup(m => m.Verify(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        var tokenService = new Mock<ITokenService>();
        tokenService.Setup(m => m.GenerateToken(It.IsAny<JwtPayload>())).Returns("token");

        var loginHandler = new LoginHandler(
            userRepository.Object,
            cryptService.Object,
            tokenService.Object
        );

        var command = new LoginCommand { Account = It.IsAny<string>(), Password = "hashedPwd" };

        var result = await loginHandler.Handle(command, CancellationToken.None);

        Assert.True(result.IsT0);
        Assert.False(result.IsT1);
    }
}
