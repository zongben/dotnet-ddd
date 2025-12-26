using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[AllowAnonymous]
public class AuthController(ISender sender) : ApiControllerBase
{
    private readonly ISender _sender = sender;

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequest req)
    {
        var command = new RegisterCommand
        {
            Account = req.Account,
            Password = req.Password,
            UserName = req.UserName,
        };
        var result = await _sender.Send(command);

        return result.Match<ActionResult>(
            (data) =>
            {
                return Ok(new OkResponse<RegisterResult>(data));
            },
            (err) =>
            {
                return Conflict(new ErrResponse(err, "account is used"));
            }
        );
    }

    [HttpPost("login")]
    [Validate<LoginRequest>]
    public async Task<ActionResult> Login([FromBody] LoginRequest req)
    {
        var command = new LoginCommand() { Account = req.Account, Password = req.Password };
        var result = await _sender.Send(command);

        return result.Match<ActionResult>(
            (data) =>
            {
                return Ok(new OkResponse<LoginResult>(data));
            },
            (err) =>
            {
                return Unauthorized(new ErrResponse(err, "account or password is wrong"));
            }
        );
    }
}
