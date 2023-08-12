using FluentResults;

using InvSys.Application.Common;
using InvSys.Application.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvSys.Application.Features.Admin.User;

[Route("/api/admin/user")]
public class UpdateUser : ApiControllerBase
{
    [HttpPut]
    public async Task<ActionResult> Modify([FromQuery] string userId, UpdateUserCommand command)
    {
        command.UserId = userId;
        Result<UpdateUserResult>? updateResult = await Mediator.Send(command);

        return updateResult.IsFailed
            ? Problem(updateResult.Errors.Select(e => e.Metadata))
            : Ok(updateResult.Value);
    }
}

public record UpdateUserResult(ApplicationUser UpdatedUser);

public record UpdateUserCommand(
    string? UserId,
    string? Email,
    string? Username,
    string? Password
) : IRequest<Result<UpdateUserResult>>
{
    public string UserId { get; set; } = UserId;
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UpdateUserResult>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateUserCommandHandler(
        UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<UpdateUserResult>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null)
        {
            Error? error = new("");

            error.Metadata.Add("UserNotFound", $"The user with an id {request.UserId} doesn't exist.");

            return Result.Fail<UpdateUserResult>(error);
        }

        UpdateUser(request, user);

        return new UpdateUserResult(user);
    }

    private void UpdateUser(UpdateUserCommand request, ApplicationUser user)
    {
        PasswordHasher<ApplicationUser>? hasher = new();

        user.Email = request.Email ?? user.Email;

        user.UserName = request.Username ?? user.UserName;

        user.PasswordHash = request.Password == default
            ? user.PasswordHash
            : hasher.HashPassword(user, request.Password);
    }
}