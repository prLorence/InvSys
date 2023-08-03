using FluentResults;

using FluentValidation;

using MediatR;

namespace InvSys.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResultBase, new()
{
    private readonly IValidator<TRequest>? _validator;

    public ValidationBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validator is null)
        {
            return await next();
        }

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next();
        }

        var resultWithErrors = new TResponse();

        foreach (var error in validationResult.Errors)
        {
            var errorDetails = new Error(error.ErrorMessage);
            errorDetails.Metadata.Add(error.ErrorCode, error.ErrorMessage);

            errorDetails.Reasons.Add(errorDetails);
        }

        return resultWithErrors;
    }
}