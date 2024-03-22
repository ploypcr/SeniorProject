namespace Contracts.Authentication;

public record UserRegisterRequest(
    string FirstName,
    string LastName,
    string StudentId,
    string Email,
    string Password
);