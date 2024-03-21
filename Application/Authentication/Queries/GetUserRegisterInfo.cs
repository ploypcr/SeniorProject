using Domain.Entities;
using MediatR;

namespace Application.Authentication.Queries;
public record GetUserRegisterInfo(
    string Token
) : IRequest<TokenResult>;