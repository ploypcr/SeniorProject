using MediatR;

namespace Application.Authentication.Commands;
public record UserRegisterCommand(
    string FirstName,
    string LastName,
    string StudentId,
    string Email,
    string Password,
    string Role
) : IRequest;