using Application.Abstractions;
using Application.Abstractions.Authentication;
using Application.Authentication.Commands;
using Domain.Entities;
using Google.Apis.Auth;
using MediatR;

namespace Application.Authentication.CommandHandlers;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, TokenResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    public RegisterCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<TokenResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(request.Token);
        var user = await _userRepository.GetUserByEmailAsync(payload.Email);
        if(user is not null){
            throw new Exception("Already has this user.");
        }

        user = User.Create(payload.GivenName, 
        payload.FamilyName, request.StudentId, payload.Email, null);
        await _userRepository.AddUserAsync(user);
        
        var token = _jwtTokenGenerator.GenerateToken(user, "User");
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        return new TokenResult(token,refreshToken, DateTime.UtcNow.AddMinutes(480));
    }
}
