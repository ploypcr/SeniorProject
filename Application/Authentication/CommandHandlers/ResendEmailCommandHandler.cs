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

public class ResendEmailCommandHandler : IRequestHandler<ResendEmailCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public ResendEmailCommandHandler(IEmailService emailService,IConfiguration configuration,IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _configuration = configuration;
        _emailService = emailService;

    }

    public async Task Handle(ResendEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email);
        if(user is null){
            throw new ArgumentNullException("Invalid user.");
        }
        if(user.EmailVerified == true){
            throw new ArgumentNullException("Invalid user.");
        }
        if(user.EmailVerificationToken == null){
            throw new ArgumentException("Invalid user.");
        }
        string emailToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        emailToken = emailToken.Replace("=","");
        emailToken = emailToken.Replace("+","");

        user.UpdateEmailToken(emailToken);
        await _userRepository.UpdateUserAsync(user);

        //await _emailService.SendEmail(request.Email, user.Id, emailToken);

    }
}
