using System.Security.Authentication;
using System.Security.Claims;
using Application.Abstractions;
using Application.Abstractions.Authentication;
using Application.Authentication;
using Application.Authentication.Commands;
using MediatR;
using Microsoft.Extensions.Configuration;

public class TokenRequestHandler : IRequestHandler<TokenRequestCommand, TokenResult>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    public TokenRequestHandler(IConfiguration configuration,IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository){
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
        _configuration = configuration;
    }
    public async Task<TokenResult> Handle(TokenRequestCommand request, CancellationToken cancellationToken)
    {
        var principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(request.accessToken);
        if(principal == null){
            throw new InvalidCredentialException("Invalid token.");
        }

        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var role = principal.FindFirstValue(ClaimTypes.Role);

        var user = await _userRepository.GetUserByIdAsync(userId);
        if(user == null){
            throw new InvalidCredentialException("Invalid token.");
        }
        foreach(var r in user.RefreshTokens){
            //Console.WriteLine(r.Token);
            if(r.CreatedTime.AddDays(1) < DateTime.UtcNow){
                user.RemoveRefreshToken(r);
            }
        }
        
        //Console.WriteLine(request.refreshToken);
        var refreshToken = user.RefreshTokens.Where(r => r.Token == request.refreshToken).FirstOrDefault();
        //Console.WriteLine(refreshToken);
        if(refreshToken == null){
            throw new InvalidCredentialException("Invalid token.");
        }

        var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();
        newRefreshToken = newRefreshToken.Replace("=","");
        newRefreshToken = newRefreshToken.Replace("+","");
        var newAccessToken = _jwtTokenGenerator.GenerateToken(user, role);
        refreshToken.Update(newRefreshToken);
        await _userRepository.UpdateUserAsync(user);

        return new TokenResult(
            newAccessToken, 
            newRefreshToken, 
            DateTime.UtcNow.AddMinutes(_configuration.GetSection("Jwt:ExpiryMinutes").Get<double>()));
    }
}