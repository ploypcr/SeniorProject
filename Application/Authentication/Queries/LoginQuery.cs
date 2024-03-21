using MediatR;

namespace Application.Authentication.Queries;
public record LoginQuery(
    string UserName,
    string Password
) : IRequest<TokenResult>;