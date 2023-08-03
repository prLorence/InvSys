using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using InvSys.Application.Common.Interfaces;
using InvSys.Application.Entities;
using InvSys.Infrastructure.Common.Security;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace InvSys.Application.Infrastructure.Security;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;
    private readonly IDateTime _dateTime;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings, IDateTime dateTime)
    {
        _jwtSettings = jwtSettings.Value;
        _dateTime = dateTime;
    }

    public string Generate(ApplicationUser user)
    {
        var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
        };

        var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: _dateTime.Now.AddDays(_jwtSettings.ExpiryDays),
                signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public Task<string> GenerateAsync(ApplicationUser user)
    {
        throw new NotImplementedException();
    }
}