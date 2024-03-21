using Application.Diagnostics.Commands;
using Application.Diagnostics.Queries;

using Contracts.Diagnostic;

using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/api/[controller]")]

public class DiagnosticController : ControllerBase{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public DiagnosticController(IMediator mediator, IMapper mapper){
        _mediator = mediator;
        _mapper = mapper;
    }
    [Authorize(Roles = "Admin,User")]
    [HttpGet()]
    public async Task<IActionResult> GetAllDiagnostics(){
        var getAllDiagnosticsResult = await _mediator.Send(new GetAllDiagnostics());
        var diagnosticResponses = getAllDiagnosticsResult.Select(d => _mapper.Map<DiagnosticResponse>(d)).ToList();
        return Ok(diagnosticResponses);
    }
    [Authorize(Roles ="Admin")]
    [HttpPost("")]
    public async Task<IActionResult> CreateDiagnostics(List<CreateDiagnosticRequest> request){
        var command = request.Select(r => _mapper.Map<CreateDiagnosticCommand>(r));
        List<DiagnosticResponse> diagnosticResponses = new();
        foreach (var c in command){
            var createDiagnosticResult = await _mediator.Send(c);
            var diagnosticResponse = _mapper.Map<DiagnosticResponse>(createDiagnosticResult);
            diagnosticResponses.Add(diagnosticResponse);
        }

        return Ok(diagnosticResponses);
    }
    [Authorize(Roles ="Admin")]
    [HttpDelete("")]
    public async Task<IActionResult> DeleteDiagnostic(List<DeleteDiagnosticRequest> request){
        var command = request.Select(r => _mapper.Map<DeleteDiagnosticCommand>(r));
        foreach (var c in command){
            await _mediator.Send(c);
        }

        return Ok(200);
    }
    [Authorize(Roles ="Admin")]
    [HttpPut("")]
    public async Task<IActionResult> UpdateDiagnostic(List<UpdateDiagnosticRequest> request){
        var command = request.Select(r => _mapper.Map<UpdateDiagnosticCommand>(r));
        List<DiagnosticResponse> diagnosticResponses = new();
        foreach (var c in command){
            var updateDiagnosticResult = await _mediator.Send(c);
            var diagnosticResponse = _mapper.Map<DiagnosticResponse>(updateDiagnosticResult);
            diagnosticResponses.Add(diagnosticResponse);
        }

        return Ok(diagnosticResponses);
    }



}