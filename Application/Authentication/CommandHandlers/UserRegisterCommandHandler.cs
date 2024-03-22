using Application.Abstractions;
using Application.Abstractions.Authentication;
using Application.Abstractions.Services;
using Application.Authentication.Commands;
using Domain.Entities;
using Google.Apis.Auth;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Authentication.CommandHandlers;

public class UserRegisterCommandHandler : IRequestHandler<UserRegisterCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public UserRegisterCommandHandler(IEmailService emailService,IConfiguration configuration,IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _configuration = configuration;
        _emailService = emailService;

    }

    public async Task Handle(UserRegisterCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email);
        if(user is not null){
            throw new Exception("Already has this user.");
        }

        user = User.Create(request.FirstName, 
        request.LastName, request.StudentId, request.Email, request.Password);
        await _userRepository.AddUserAsync(user);

        await _emailService.SendEmail(request.Email, "Hello");

        // var token = _jwtTokenGenerator.GenerateToken(user, "User");
        // var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        // var userRefreshToken = RefreshToken.Create(refreshToken);
        // user.AddRefreshToken(userRefreshToken);
        // await _userRepository.UpdateUserAsync(user);


        // return new TokenResult(
        //     token,
        //     refreshToken, 
        //     DateTime.UtcNow.AddMinutes(_configuration.GetSection("Jwt:ExpiryMinutes").Get<double>()));
    }
}
