using Api.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection{
    public static IServiceCollection AddPresentation(this IServiceCollection services){
        services.AddMappings();
        services.AddControllers();
        return services;
    }
}