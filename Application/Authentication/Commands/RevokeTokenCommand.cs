using Domain.Entities;
using MediatR;

namespace Application.Authentication.Commands;
public record RevokeTokenCommand(
    string accessToken,
    string refreshToken
) : IRequest;