using Domain.Entities;
using MediatR;

namespace Application.Authentication.Queries;
public record GetUserInfo(
    string Token
) : IRequest<User>;