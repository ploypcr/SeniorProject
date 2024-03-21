using MediatR;

namespace Application.Authentication.Commands;
public record RegisterCommand(
    string FirstName,
    string LastName,
    string StudentId,
    string Token
) : IRequest<TokenResult>;