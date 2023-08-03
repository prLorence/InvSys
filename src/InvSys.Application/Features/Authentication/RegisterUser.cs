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
        // return await Mediator.Send();
        var registerResult = await Mediator.Send(command);

        if (registerResult.IsFailed)
        {
            // this line is fugly
            // return Problem(registerResult.Errors.Select(
            //             e => new Dictionary<string, object>()
            //             {
            //                         { e., e.Message }
            // }));
            return Problem(registerResult.Reasons.Select(
                        r => new Dictionary<string, object>()
                        {
                            { "Details", r.Message }
                        }
            ));
            // var errors = registerResult.Errors.Select(e => e.Metadata);
            // return Problem(errors);
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

        var identityUser = await _userManager.CreateAsync(newUser, request.Password);

        if (!identityUser.Succeeded)
        {
            return Result.Fail<RegisterUserResult>(identityUser.Errors.Select(e => e.Description));
        }

        var jwt = _jwtTokenGenerator.Generate(newUser);

        return new RegisterUserResult(
                newUser,
                jwt);
    }
}