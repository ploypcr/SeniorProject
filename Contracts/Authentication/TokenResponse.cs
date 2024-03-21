namespace Contracts.Authentication;
public record TokenResponse(
    string accessToken,
    string refreshToken
);