using InvSys.Application.Common;

using Microsoft.AspNetCore.Mvc;

namespace InvSys.Application.Features.Authentication;

[Route("api/auth/[action]")]
public class RegisterUserController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Register()
    {
        // return await Mediator.Send();
        await Task.CompletedTask;

        return Ok();
    }
}

