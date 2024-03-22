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
}