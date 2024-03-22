using Application.Treatments.Commands;
using Application.Treatments.Queries;
using Contracts.Treatment;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TreatmentController : ControllerBase{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public TreatmentController(IMapper mapper, IMediator mediator){
        _mediator = mediator;
        _mapper = mapper;
    }

    [Authorize(Roles ="Admin")]
    [HttpPost("")]
    public async Task<IActionResult> CreateTreatment(List<CreateTreatmentRequest> request){
        var command  = request.Select(r => _mapper.Map<CreateTreatmentCommand>(r));
        List<TreatmentResponse> treatmentResponses = new();
        foreach (var c in command){
            var createTreatmentResult = await _mediator.Send(c);
            var treatmentResponse = _mapper.Map<TreatmentResponse>(createTreatmentResult);
            treatmentResponses.Add(treatmentResponse);
        }
        return Ok(treatmentResponses);
    }

    [Authorize(Roles = "Admin,User")]
    [HttpGet()]
    public async Task<IActionResult> GetAllTreatments(){
        var treatmentResult = await _mediator.Send(new GetAllTreatments());
        var treatmentResponse = treatmentResult.Select(t => _mapper.Map<TreatmentResponse>(t)).ToList();
        return Ok(treatmentResponse);
    }
    [Authorize(Roles ="Admin")]
    [HttpDelete()]
    public async Task<IActionResult> DeleteTreatment(List<DeleteTreatmentRequest> request){
        var command  = request.Select(r => _mapper.Map<DeleteTreatmentCommand>(r));
        foreach(var c in command){
            await _mediator.Send(c);
        }
        return Ok(200);
    }
    [Authorize(Roles ="Admin")]
    [HttpPut()]
    public async Task<IActionResult> UpdateTreatment(List<UpdateTreatmentRequest> request){
        var command = request.Select(r => _mapper.Map<UpdateTreatmentCommand>(r));
        List<TreatmentResponse> treatmentResponses = new();
        foreach (var c in command){
            var updateTreatmentResult = await _mediator.Send(c);
            var treatmentResponse = _mapper.Map<TreatmentResponse>(updateTreatmentResult);
        }
        return Ok(treatmentResponses);
    }
}