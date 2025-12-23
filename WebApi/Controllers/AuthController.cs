using ApplicationLayer.UseCases.Auth.Command.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contract.Auth;

[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
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
                return Ok(data);
            },
            (err) =>
            {
                return Conflict();
            }
        );
    }
}
