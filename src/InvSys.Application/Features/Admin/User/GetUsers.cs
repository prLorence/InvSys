using System.Collections;

using FluentResults;

using InvSys.Application.Common;
using InvSys.Application.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvSys.Application.Features.Admin.User;

[Route("/api/admin/users")]
public class GetUsersController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        Result<AllUsersQueryResult> queryResult = await Mediator.Send(new AllUsersQuery());

        return Ok(queryResult.Value);
    }
}

public record AllUsersQuery() : IRequest<Result<AllUsersQueryResult>>;

public record AllUsersQueryResult(
    List<UsersViewModel> users);

public record UsersViewModel(
    Guid Id,
    string Username,
    string Email,
    string Roles)
{
    public Guid Id { get; set; } = Id;
    public string Username { get; set; } = Username;
    public string Email { get; set; } = Email;
    public string Roles { get; set; } = Roles;
}

public class GetUsers : IRequestHandler<AllUsersQuery, Result<AllUsersQueryResult>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUsers(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<AllUsersQueryResult>> Handle(AllUsersQuery request, CancellationToken cancellationToken)
    {
        List<ApplicationUser> users = await _userManager.Users
            .ToListAsync(cancellationToken);

        List<UsersViewModel> userViewModels = new();

        foreach (ApplicationUser user in users)
        {
            IList<string> roles = await _userManager.GetRolesAsync(user);
            UsersViewModel userViewModel = new(
                user.Id,
                user.UserName!,
                user.Email!,
                string.Join(",", roles)
            );
            userViewModels.Add(userViewModel);
        }

        return new AllUsersQueryResult(userViewModels);
    }
}