using FluentValidation;

public static class ValidatorDependencyInjection
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
        return services;
    }
}
