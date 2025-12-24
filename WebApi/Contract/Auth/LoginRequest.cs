using FluentValidation;

public record LoginRequest
{
    public required string Account { get; init; }
    public required string Password { get; init; }
}

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Account).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
