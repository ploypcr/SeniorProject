using MediatR;

namespace Application.Authentication.Commands;
public record ConfirmEmailCommand(
    string Id,
    string Token
) : IRequest;