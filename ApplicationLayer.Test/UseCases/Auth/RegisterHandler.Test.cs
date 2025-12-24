using Moq;

public class RegisterHandlerTest
{
    [Fact]
    public async Task UserIsExistsShouldFailed()
    {
        var user = User.Create(UserId.Create(""), "account", "hashedPwd", "username");

        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(m => m.GetUserByAccount(It.IsAny<string>())).ReturnsAsync(user);

        var cryptService = new Mock<ICryptService>();

        var registerHandler = new RegisterHandler(userRepository.Object, cryptService.Object);

        var command = new RegisterCommand
        {
            Account = "Account",
            Password = "Password",
            UserName = "UserName",
        };

        var result = await registerHandler.Handle(command, CancellationToken.None);

        Assert.False(result.IsT0);
        Assert.True(result.IsT1);
        Assert.Equal(ERROR_CODES.ACCOUNT_IS_USED, result.AsT1.Code);
    }

    [Fact]
    public async Task Success()
    {
        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(m => m.GetUserByAccount(It.IsAny<string>())).ReturnsAsync((User?)null);

        var cryptService = new Mock<ICryptService>();

        var registerHandler = new RegisterHandler(userRepository.Object, cryptService.Object);

        var command = new RegisterCommand
        {
            Account = "Account",
            Password = "Password",
            UserName = "UserName",
        };

        var result = await registerHandler.Handle(command, CancellationToken.None);

        Assert.True(result.IsT0);
        Assert.False(result.IsT1);
        userRepository.Verify(m => m.Create(It.IsAny<User>()), Times.Once);
    }
}
