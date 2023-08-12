using System.Reflection;
using System.Security.Claims;

using FluentResults;

using InvSys.Application.Common;
using InvSys.Application.Common.Interfaces;
using InvSys.Application.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InvSys.Application.Features.Authentication;

[Route("api/auth/[action]")]
public class RegisterUserController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Register(RegisterUserCommand command)
    {
        var registerResult = await Mediator.Send(command);

        if (registerResult.IsFailed)
        {
            return Problem(registerResult.Errors.Select(e => e.Metadata));
        }

        return Ok(registerResult.Value);
    }
}

public record RegisterUserResult(
        ApplicationUser User,
        string Token);

public record RegisterUserCommand(
        string Username,
        string Email,
        string Password) : IRequest<Result<RegisterUserResult>>;

public class RegisterUser : IRequestHandler<RegisterUserCommand, Result<RegisterUserResult>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterUser(
        UserManager<ApplicationUser> userManager,
        IJwtTokenGenerator jwtTokenGenerator,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _signInManager = signInManager;
    }

    public async Task<Result<RegisterUserResult>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var newUser = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            UserName = request.Username,
            NormalizedEmail = request.Email.ToUpper(),
            NormalizedUserName = request.Username.ToUpper()
        };

        var registerResult = await _userManager.CreateAsync(newUser, request.Password);

        if (!registerResult.Succeeded)
        {
            var details = new Error(registerResult.ToString());

            foreach (var error in registerResult.Errors)
            {
                details.Metadata.Add(error.Code, error.Description);
            }

            return Result.Fail<RegisterUserResult>(details);
        }

        var jwt = _jwtTokenGenerator.Generate(newUser);

        return new RegisterUserResult(
                newUser,
                jwt);
    }
}