using System.Security.Authentication;
using Application.Abstractions;
using Application.Authentication.Queries;
using Domain.Entities;
using Google.Apis.Auth;
using MediatR;

namespace Application.Authentication.QueryHandlers;

public class GetUserInfoHandler : IRequestHandler<GetUserInfo, User>
{
    private readonly IUserRepository _userRepository;

    public GetUserInfoHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Handle(GetUserInfo request, CancellationToken cancellationToken)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(request.Token);
        if(payload.HostedDomain != "ku.th"){
            throw new InvalidCredentialException("Email is not in ku.th domain");
        }
        var user = await _userRepository.GetUserByEmailAsync(payload.Email);

        return user;
    }
}
