using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;

namespace InvSys.Application.Common;

[ApiController]
public class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>()!;

    protected ActionResult Problem(IEnumerable<IDictionary<string, object>> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();

        var errorMap = errors
          .SelectMany(dictionary => dictionary)
          .Select(item => new KeyValuePair<string, object>(item.Key, item.Value));

        foreach (KeyValuePair<string, object> error in errorMap)
        {
            modelStateDictionary.AddModelError(error.Key, error.Value.ToString() ?? "False Error (help me)");
        }

        return ValidationProblem(modelStateDictionary);
    }

    protected ActionResult Problem(IEnumerable<string> messages)
    {
        var modelStateDictionary = new ModelStateDictionary();

        foreach (var message in messages)
        {
            modelStateDictionary.AddModelError(message, message ?? "False Error (help me)");
        }

        return ValidationProblem(modelStateDictionary);
    }

}