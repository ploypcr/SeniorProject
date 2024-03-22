using MediatR;

namespace Application.Authentication.Commands;
public record ResendEmailCommand(
    string Email
) : IRequest;