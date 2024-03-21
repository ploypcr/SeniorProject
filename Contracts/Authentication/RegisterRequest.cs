namespace Contracts.Authentication;

public record RegisterRequest(
    string FirstName,
    string LastName,
    string StudentId
);