using System.Security.Authentication;
using Application.Abstractions;
using Application.Abstractions.Authentication;
using Application.Authentication.Queries;
using Google.Apis.Auth;
using MediatR;

namespace Application.Authentication.QueryHandlers;

public class VerifyUserDomainHandler : IRequestHandler<VerifyUserDomain, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;


    public VerifyUserDomainHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<bool> Handle(VerifyUserDomain request, CancellationToken cancellationToken)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(request.Token);
        if(payload.HostedDomain != "ku.th"){
            throw new InvalidCredentialException("Email is not in ku.th domain");
        }
        return true;
    }
}
