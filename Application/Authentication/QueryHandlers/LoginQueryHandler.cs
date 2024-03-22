using Application.Abstractions;
using Application.Abstractions.Authentication;
using Application.Authentication.Queries;
using MediatR;

namespace Application.Authentication.QueryHandlers;

public class LoginQueryHandler : IRequestHandler<LoginQuery, TokenResult>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository){
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }
    public async Task<TokenResult> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.UserName);

        if(user == null){
            throw new ArgumentException ("Don't have this user");
        }

        if (user.Password != request.Password){
            throw new ArgumentException ("Password doesn't match.");
        }

        foreach(var r in user.RefreshTokens){
            if(r.CreatedTime.AddDays(1) < DateTime.UtcNow){
                user.RemoveRefreshToken(r);
            }
        }

        var token = _jwtTokenGenerator.GenerateToken(user, "Admin");
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        
        return new TokenResult(token, refreshToken, DateTime.Now.AddMinutes(480));
    }
}