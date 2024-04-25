using System.Security.Authentication;
using Application.Abstractions;
using Application.Abstractions.Authentication;
using Application.Abstractions.Services;
using Application.Authentication.Commands;
using Domain.Entities;
using Google.Apis.Auth;
using MediatR;
using Microsoft.Extensions.Configuration;
using BC = BCrypt.Net.BCrypt;
namespace Application.Authentication.CommandHandlers;

public class UserLoginHandler : IRequestHandler<UserLogin, TokenResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IEmailService _emailService;

    private readonly IConfiguration _configuration;
    public UserLoginHandler(IEmailService emailService,IConfiguration configuration,IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _configuration = configuration;
        _emailService = emailService;
    }

    public async Task<TokenResult> Handle(UserLogin request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email);

        if(user == null){
            throw new InvalidCredentialException("Invalid user or password.");
        }

        if (!BC.Verify(request.Password, user.Password)){
            throw new InvalidCredentialException("Invalid user or password.");
        }

        if(user.EmailVerified == false){

            string emailToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            emailToken = emailToken.Replace("=","");
            emailToken = emailToken.Replace("+","");
            user.UpdateEmailToken(emailToken);
            await _userRepository.UpdateUserAsync(user);
            //await _emailService.SendEmail(user.Email, user.Id, emailToken);

            throw new ArgumentException("Please confirm your email first. we resend a link to your email.");
        }

        foreach(var r in user.RefreshTokens){
            if(r.CreatedTime.AddDays(1) < DateTime.UtcNow){
                user.RemoveRefreshToken(r);
            }
        }


        var token = _jwtTokenGenerator.GenerateToken(user, "User");
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();
        refreshToken = refreshToken.Replace("=","");
        refreshToken = refreshToken.Replace("+","");
        
        var userRefreshToken = RefreshToken.Create(refreshToken, user.Id);
        user.AddRefreshToken(userRefreshToken);
        await _userRepository.UpdateUserAsync(user);

        
        return new TokenResult(
            token, 
            refreshToken, 
            DateTime.UtcNow.AddMinutes(_configuration.GetSection("Jwt:ExpiryMinutes").Get<double>()));
    }
}
