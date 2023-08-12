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
public class CreateUserController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult> CreateUser(CreateUserCommand command)
    {
        Result<CreateUserResult> registerResult = await Mediator.Send(command);

        return registerResult.IsFailed
            ? Problem(registerResult.Errors.Select(e => e.Metadata))
            : Ok(registerResult.Value);
    }
}

public record CreateUserResult(
    string message);

public record CreateUserCommand(
    string Email,
    string Username,
    string Password,
    List<string> Roles) : IRequest<Result<CreateUserResult>>;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<CreateUserResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;


    public CreateUserCommandHandler(
        UserManager<ApplicationUser> userManager,
        IJwtTokenGenerator jwtTokenGenerator,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _roleManager = roleManager;
    }

    public async Task<Result<CreateUserResult>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser newUser = new()
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            UserName = request.Username,
            NormalizedEmail = request.Email.ToUpper(),
            NormalizedUserName = request.Username.ToUpper()
        };

        foreach (string role in request.Roles)
        {
            bool isValidRole = await _roleManager.RoleExistsAsync(role);

            if (!isValidRole)
            {
                Error details = new(role);

                details.Metadata.Add("InvalidRole", $"{role} doesn't exist.");

                return Result.Fail(details);
            }
        }

        IdentityResult registerResult = await _userManager.CreateAsync(newUser, request.Password);

        if (!registerResult.Succeeded)
        {
            Error details = new(registerResult.ToString());

            foreach (IdentityError error in registerResult.Errors)
            {
                details.Metadata.Add(error.Code, error.Description);
            }

            return Result.Fail<CreateUserResult>(details);
        }

        IdentityResult userRole = await _userManager.AddToRolesAsync(newUser, request.Roles);

        if (!userRole.Succeeded)
        {
            Error details = new(userRole.ToString());

            foreach (IdentityError error in userRole.Errors)
            {
                details.Metadata.Add(error.Code, error.Description);
            }

            return Result.Fail<CreateUserResult>(details);
        }

        string jwt = await _jwtTokenGenerator.GenerateAsync(newUser);

        return new CreateUserResult("Created User");
    }
}