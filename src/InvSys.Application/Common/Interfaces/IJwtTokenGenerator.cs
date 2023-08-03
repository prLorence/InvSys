using System.Security.Claims;

using InvSys.Application.Entities;

namespace InvSys.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    Task<string> GenerateAsync(ApplicationUser user);
    string Generate(ApplicationUser user);
}