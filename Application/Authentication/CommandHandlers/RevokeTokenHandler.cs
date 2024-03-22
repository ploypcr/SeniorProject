using System.Security.Authentication;
using System.Security.Claims;
using Application.Abstractions;
using Application.Abstractions.Authentication;
using Application.Authentication.Commands;
using MediatR;

public class RevokeTokenHandler : IRequestHandler<RevokeTokenCommand>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    public RevokeTokenHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository){
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;

    }
    public async Task Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
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

        user.RemoveRefreshToken(refreshToken);
        await _userRepository.UpdateUserAsync(user);
    }
}