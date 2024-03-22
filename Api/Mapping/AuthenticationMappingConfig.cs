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
    }
}