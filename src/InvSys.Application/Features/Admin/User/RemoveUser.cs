using FluentResults;

using InvSys.Application.Common;
using InvSys.Application.Common.Interfaces;
using InvSys.Application.Common.Roles;
using InvSys.Application.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InvSys.Application.Features.Admin.User;

[Route("api/admin/[action]")]
public class RemoveUserController : ApiControllerBase
{
    [HttpDelete]
    public async Task<ActionResult> RemoveUser([FromQuery] RemoveUserCommand command)
    {
        var removeResult = await Mediator.Send(command);

        if (removeResult.IsFailed)
        {
            return Problem(removeResult.Errors.Select(e => e.Metadata));
        }

        return NoContent();
    }
}

public record RemoveUserCommand(string Username) : IRequest<Result>;

public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand, Result>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RemoveUserCommandHandler(
        UserManager<ApplicationUser> userManager,
        IJwtTokenGenerator jwtTokenGenerator,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _roleManager = roleManager;
    }

    public async Task<Result> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        if (user is null)
        {
            var details = new Error("");

            details.Metadata.Add("UserNotFound", $"User with username {request.Username} isn't found.");

            return Result.Fail(details);
        }

        var userRoles = await _userManager.GetRolesAsync(user!);

        var revokeRoles = await _userManager.RemoveFromRolesAsync(user!, userRoles);

        if (!revokeRoles.Succeeded)
        {
            var details = new Error(revokeRoles.ToString());

            foreach (var error in revokeRoles.Errors)
            {
                details.Metadata.Add(error.Code, error.Description);
            }

            return Result.Fail(details);
        }

        var removeResult = await _userManager.DeleteAsync(user!);

        if (!removeResult.Succeeded)
        {
            var details = new Error(removeResult.ToString());

            foreach (var error in removeResult.Errors)
            {
                details.Metadata.Add(error.Code, error.Description);
            }

            return Result.Fail(details);
        }

        return Result.Ok();
    }
}