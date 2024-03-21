using System.Security.Authentication.ExtendedProtection;
using Application.Authentication.Commands;
using Application.Authentication.Queries;
using Contracts.Authentication;
using Domain.Entities;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/api")]
[Authorize(Policy = "OnlyGoogleJwtScheme")]
public class GoogleAuthenticationController : ControllerBase{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator; 

    public GoogleAuthenticationController(IMediator mediator, IMapper mapper){
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request){
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var command  = new RegisterCommand(request.FirstName, request.LastName, request.StudentId,token);
        var accessToken = await _mediator.Send(command);
        return Ok(accessToken);
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin(){
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var command  = new GetUserRegisterInfo(token);
        var res = await _mediator.Send(command);
        return Ok(res);
    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyUser(){
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var command  = new VerifyUserDomain(token);
        var res = await _mediator.Send(command);
        return Ok(res);
    }
}