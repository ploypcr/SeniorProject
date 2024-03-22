using Application.Authentication.Commands;
using Contracts.Authentication;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/api")]
public class AuthController : ControllerBase{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator; 
    public AuthController(IMediator mediator, IMapper mapper){
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenRequest request){
        var command  = new TokenRequestCommand(request.accessToken , request.refreshToken);
        var token = await _mediator.Send(command);
        return Ok(token);
    }

    [HttpPost("user-register")]
    public async Task<IActionResult> RegisterUser(UserRegisterRequest request){
        var command  = new UserRegisterCommand(request.FirstName , request.LastName, request.StudentId, request.Email, request.Password);
        await _mediator.Send(command);
        return Ok("Sent to email successfully");
    }

    [HttpPost("user-login")]
    public async Task<IActionResult> LoginUser(LoginRequest request){
        var command  = new UserLogin(request.UserName , request.Password);
        var token = await _mediator.Send(command);
        return Ok(token);
    }

    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken(TokenRequest request){
        var command  = new RevokeTokenCommand(request.accessToken , request.refreshToken);
        await _mediator.Send(command);
        return Ok(200);
    }

    [HttpGet("confirmEmail")]
    public async Task<IActionResult> EmailConfirm([FromQuery] string id, [FromQuery] string code){
        Console.WriteLine(id, code);
        var command  = new ConfirmEmailCommand(id , code);
        await _mediator.Send(command);
        return Ok(200);
    }
}