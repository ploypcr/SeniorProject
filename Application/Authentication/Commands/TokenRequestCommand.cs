using Domain.Entities;
using MediatR;

namespace Application.Authentication.Commands;
public record TokenRequestCommand(
    string accessToken,
    string refreshToken
) : IRequest<TokenResult>;