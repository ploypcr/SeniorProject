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
public class AdminController : ControllerBase{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator; 
    public AdminController(IMediator mediator, IMapper mapper){
        _mediator = mediator;
        _mapper = mapper;
    }

    // [HttpPost("seed")]
    // public async Task<IActionResult> SeedAdmin(){
    //     var command  = new GenerateAdminCommand("admin1","","admin1","Xaw8rocQJ0tkLBV");
    //     await _mediator.Send(command);
    //     return Ok(200);
    // }

    [HttpPost("admin-login")]
    public async Task<IActionResult> AdminLogin(LoginRequest request){
        var command  = new LoginQuery(request.UserName,request.Password);
        var token = await _mediator.Send(command);
        return Ok(token);
    }
}