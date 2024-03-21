using Application.Problems.Commands;
using Application.Problems.Queries;
using Contracts.Problem;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/api/[controller]")]

public class ProblemController : ControllerBase{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public ProblemController(IMapper mapper, IMediator mediator){
        _mediator = mediator;
        _mapper = mapper;
    }
    [Authorize(Roles ="Admin")]
    [HttpPost("")]
    public async Task<IActionResult> CreateProblem(List<CreateProblemRequest> request){
        var command = request.Select(r => _mapper.Map<CreateProblemCommand>(r));
        List<ProblemResponse> problemResponses = new();
        foreach (var c in command){
            var createProblemResult = await _mediator.Send(c);
            var problemResponse = _mapper.Map<ProblemResponse>(createProblemResult);
            problemResponses.Add(problemResponse);
        }


        return Ok(problemResponses);
    }

    [Authorize(Roles = "Admin,User")]
    [HttpGet()]
    public async Task<IActionResult> GetAllProblem(){
        var problemsResult = await _mediator.Send(new GetAllProblems());
        var problemsResponse = problemsResult.Select(p => _mapper.Map<ProblemResponse>(p)).ToList();
        return Ok(problemsResponse);
    }
    [Authorize(Roles ="Admin")]
    [HttpDelete("")]
   public async Task<IActionResult> DeleteProblem(List<DeleteProblemRequest> request){
        var command = request.Select(r => _mapper.Map<DeleteProblemCommand>(r));
        foreach (var c in command){
            await _mediator.Send(c);
        }

        return Ok(200);
    }

    [Authorize(Roles ="Admin")]
    [HttpPut("")]
    public async Task<IActionResult> UpdateProblem(List<UpdateProblemRequest> request){
        var command = request.Select(r => _mapper.Map<UpdateProblemCommand>(r));
        List<ProblemResponse> problemResponses = new();
        foreach (var c in command){
            var updateProblmResult = await _mediator.Send(c);
            var problemResponse = _mapper.Map<ProblemResponse>(updateProblmResult);
            problemResponses.Add(problemResponse);
        }


        return Ok(problemResponses);
    }
}