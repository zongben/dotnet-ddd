using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[AllowAnonymous]
public class AuthController : ApiControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequest body)
    {
        var command = new RegisterCommand
        {
            Account = body.Account,
            Password = body.Password,
            UserName = body.UserName,
        };
        var result = await _sender.Send(command);

        return result.Match<ActionResult>(
            (data) =>
            {
                return Ok(new OkResponse<RegisterResult>(data));
            },
            (err) =>
            {
                return Conflict(new ErrResponse(err));
            }
        );
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest body)
    {
        var command = new LoginCommand() { Account = body.Account, Password = body.Password };
        var result = await _sender.Send(command);

        return result.Match<ActionResult>(
            (data) =>
            {
                return Ok(new OkResponse<LoginResult>(data));
            },
            (err) =>
            {
                return Unauthorized(new ErrResponse(err));
            }
        );
    }
}
