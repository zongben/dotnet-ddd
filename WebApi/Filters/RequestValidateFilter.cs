using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class RequestValidateFilter<T>(IValidator<T> validator) : IActionFilter
{
    private readonly IValidator<T> _validator = validator;

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var request = context.ActionArguments.Values.OfType<T>().FirstOrDefault();

        if (request == null)
        {
            return;
        }

        var result = _validator.Validate(request);

        if (!result.IsValid)
        {
            context.Result = new BadRequestObjectResult(
                ErrResponse.RequestInvalid(result.Errors.FirstOrDefault()?.ToString() ?? "")
            );
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class ValidateAttribute<TRequest> : TypeFilterAttribute
{
    public ValidateAttribute()
        : base(typeof(RequestValidateFilter<TRequest>)) { }
}
