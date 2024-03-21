using Domain.Entities;
using MediatR;

namespace Application.Authentication.Queries;
public record VerifyUserDomain(
    string Token
) : IRequest<bool>;