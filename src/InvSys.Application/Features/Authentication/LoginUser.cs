using FluentResults;

using InvSys.Application.Common;
using InvSys.Application.Common.Interfaces;
using InvSys.Application.Entities;
using InvSys.Application.Infrastructure;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InvSys.Application.Features.Authentication;

[Route("/api/auth/login")]
public class LoginUserController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Login(LoginQuery query)
    {
        var loginResult = await Mediator.Send(query);

        if (loginResult.IsFailed)
        {
            return Problem(loginResult.Errors.Select(e => e.Message));
        }

        return Ok(loginResult.Value);
    }
}

public record LoginQueryResult(ApplicationUser User, string Token);

public record LoginQuery(string Username, string Password) : IRequest<Result<LoginQueryResult>>;

public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<LoginQueryResult>>
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly InvSysDbContext _dbContext;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly ILogger<LoginQueryHandler> _logger;

    public LoginQueryHandler(
        SignInManager<ApplicationUser> signInManager,
        InvSysDbContext dbContext,
        IJwtTokenGenerator tokenGenerator,
        UserManager<ApplicationUser> userManager,
        ILogger<LoginQueryHandler> logger)
    {
        _signInManager = signInManager;
        _dbContext = dbContext;
        _tokenGenerator = tokenGenerator;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result<LoginQueryResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        if (user is null)
        {
            return Result.Fail<LoginQueryResult>($"User with username {request.Username} is not found");
        }

        var signInResult = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!signInResult)
        {
            return Result.Fail<LoginQueryResult>($"Incorrect password");
        }

        var token = _tokenGenerator.Generate(user!);

        return new LoginQueryResult(user, token);
    }
}