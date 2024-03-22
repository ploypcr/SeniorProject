using System.Text;
using Application.Abstractions;
using Application.Abstractions.Authentication;
using Application.Abstractions.Services;
using Domain.Entities;
using Infrastructure.Authentication;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
namespace Infrastructure;
public static class DependencyInjection{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration){
        services.AddAuth(configuration);
        services.AddSingleton<IDateTimeProvider,DateTimeProvider>();

        services.AddPersistence(configuration);
        services.AddStorage();
        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services, ConfigurationManager configuration){
        services.AddSingleton<IJwtTokenGenerator,JwtTokenGenerator>();
        
        services.AddAuthentication(options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer( options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters{
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration.GetSection("Jwt:Issuer").Get<string>(),
                    ValidAudience = configuration.GetSection("Jwt:Audience").Get<string>(),
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration.GetSection("Jwt:Secret").Get<string>())
                    )
                };
            })
            .AddJwtBearer("GoogleJWT",options => {
                options.Authority = "https://accounts.google.com";
                options.Audience = configuration.GetSection("Authentication:Google:ClientId").Value;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuers = new List<string>{"https://accounts.google.com","accounts.google.com"},
                };
            });
            
        services.AddAuthorization(options =>
        {
            var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                JwtBearerDefaults.AuthenticationScheme,
                "GoogleJWT");
            defaultAuthorizationPolicyBuilder =
                defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
            options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();

            var onlyGoogleJwtSchemePolicyBuilder = new AuthorizationPolicyBuilder("GoogleJWT");
            options.AddPolicy("OnlyGoogleJwtScheme", onlyGoogleJwtSchemePolicyBuilder
            .RequireAuthenticatedUser()
            .Build());
        });

        // services.AddAuthentication(options => {
        //     options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //     options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        // })
        //     .AddCookie()
        //     .AddGoogle(GoogleDefaults.AuthenticationScheme,googleOptions =>{
        //     googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
        //     googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
        //     });
        return services;
    }
    public static IServiceCollection AddPersistence(this IServiceCollection services, ConfigurationManager configuration){
        services.AddDbContext<HistoryTakingDb>(options => options.UseSqlServer(configuration.GetConnectionString("MSSQL")));
        //services.AddHealthChecks().AddDbContextCheck<HistoryTakingDb>();
        //services.AddIdentityCore<User>().AddEntityFrameworkStores<HistoryTakingDb>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IDiagnosticRepository,DiagnosticRepository>();
        services.AddScoped<IProblemRepository,ProblemRepository>();
        services.AddScoped<ITreatmentRepository,TreatmentRepository>();
        services.AddScoped<ITagRepository,TagRepository>();
        services.AddScoped<IExaminationRepository,ExaminationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IStatsRepository, StatsRepository>();
        services.AddScoped<IEmailService, EmailService>();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        

        return services;
    }

    public static IServiceCollection AddStorage(this IServiceCollection services){
        services.AddScoped<IFileStorageService, FileStorageService>();
        return services;
    }
}