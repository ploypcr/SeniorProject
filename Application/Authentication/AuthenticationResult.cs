using Domain.Entities;

namespace Application.Authentication;
public record AuthenticationResult(
    User User,
    string Token
);

