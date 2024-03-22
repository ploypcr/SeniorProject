using Application.Abstractions;
using Application.Abstractions.Authentication;
using Application.Authentication.Commands;
using Domain.Entities;
using Google.Apis.Auth;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Authentication.CommandHandlers;

public class UserLoginHandler : IRequestHandler<UserLogin, TokenResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IConfiguration _configuration;
    public UserLoginHandler(IConfiguration configuration,IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _configuration = configuration;
    }

    public async Task<TokenResult> Handle(UserLogin request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email);

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

        var token = _jwtTokenGenerator.GenerateToken(user, "User");
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();
        
        var userRefreshToken = RefreshToken.Create(refreshToken);
        user.AddRefreshToken(userRefreshToken);
        await _userRepository.UpdateUserAsync(user);

        
        return new TokenResult(
            token, 
            refreshToken, 
            DateTime.UtcNow.AddMinutes(_configuration.GetSection("Jwt:ExpiryMinutes").Get<double>()));
    }
}
