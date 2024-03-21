using System.Security.Claims;
using Domain.Entities;

namespace Application.Abstractions.Authentication;

public interface IJwtTokenGenerator{
    string GenerateToken(User user, string role);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
};