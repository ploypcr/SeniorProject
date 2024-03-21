namespace Contracts.Authentication;

public record TokenRequest(
    string accessToken,
    string refreshToken
);