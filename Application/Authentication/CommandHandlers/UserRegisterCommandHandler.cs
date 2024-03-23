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
            throw new ArgumentException("Already has this user.");
        }
        if(request.Email.Split("@")[1] != "ku.th"){
            throw new ArgumentException("This user is not in ku.th domain");
        }

        string emailToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        emailToken = emailToken.Replace("=","");
        emailToken = emailToken.Replace("+","");
        var salt = BC.GenerateSalt(10);
        var hashedPassword = BC.HashPassword(request.Password, salt);

        user = User.Create(request.FirstName, 
        request.LastName, request.StudentId, request.Email, hashedPassword, emailToken, true);
        await _userRepository.AddUserAsync(user);

        //await _emailService.SendEmail(request.Email, user.Id, emailToken);

    }
}
