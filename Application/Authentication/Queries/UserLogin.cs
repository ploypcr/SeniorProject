using MediatR;

namespace Application.Authentication.Commands;
public record UserLogin(
    string Email,
    string Password
) : IRequest<TokenResult>;