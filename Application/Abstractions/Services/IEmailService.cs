using Application.Questions.Commands;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions.Services;

public interface IEmailService{
    Task SendEmail( string To, string body);
}