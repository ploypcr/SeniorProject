using Application.Authentication;
using Application.Authentication.Commands;
using Application.Authentication.Queries;
using Contracts.Authentication;
using Mapster;

namespace Api.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>().Map(dest => dest, src => src);
        config.NewConfig<LoginRequest, LoginQuery>().Map(dest => dest, src => src);
        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest.FirstName, src => src.User.FirstName)
            .Map(dest => dest.LastName, src => src.User.LastName)
            .Map(dest => dest.Email, src => src.User.Email)
            .Map(dest => dest.Token, src => src.Token);
    }
}