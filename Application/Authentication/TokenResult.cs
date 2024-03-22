
namespace Application.Authentication;
public record TokenResult(
    string AccessToken,
    string RefreshToken,
    DateTime TokenExpires
);

