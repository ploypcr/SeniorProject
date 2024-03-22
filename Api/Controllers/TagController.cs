using Application.Tags.Commands;
using Application.Tags.Queries;
using Contracts.Tag;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/api/[controller]")]


public class TagController : ControllerBase{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public TagController(IMediator mediator, IMapper mapper){
        _mapper = mapper;
        _mediator = mediator;
    }

    [Authorize(Roles = "Admin,User")]
    [HttpGet()]
    public async Task<IActionResult> GetAllTags(){
        var getAllTagsResult = await _mediator.Send(new GetAllTags());
        var tagsResult = getAllTagsResult.Select(t => _mapper.Map<TagResponse>(t)).ToList();
        return Ok(tagsResult);
    }
    [Authorize(Roles ="Admin")]
    [HttpPost("")]
    public async Task<IActionResult> CreateTag(List<CreateTagRequest> request){
        var command = request.Select(r => _mapper.Map<CreateTagCommand>(r));
        List<TagResponse> tagResponses = new();
        foreach (var c in command){
            var createTagResult = await _mediator.Send(c);
            var tagResponse = _mapper.Map<TagResponse>(createTagResult);
            tagResponses.Add(tagResponse);
        }

        return Ok(tagResponses);
    }
    [Authorize(Roles ="Admin")]
    [HttpDelete("")]
    public async Task<IActionResult> DeleteTag(List<DeleteTagRequest> request){
        var command = request.Select(r => _mapper.Map<DeleteTagCommand>(r));
        foreach (var c in command){
            await _mediator.Send(c);
        }

        return Ok(200);
    }
    [Authorize(Roles ="Admin")]
    [HttpPut("")]
    public async Task<IActionResult> UpdateTag(List<UpdateTagRequest> request){
        var command = request.Select(r => _mapper.Map<UpdateTagCommand>(r));
        List<TagResponse> tagResponses = new();
        foreach (var c in command){
            var updateTagResult = await _mediator.Send(c);
            var tagResponse = _mapper.Map<TagResponse>(updateTagResult);
            tagResponses.Add(tagResponse);
        }

        return Ok(tagResponses);
    }
}
