using Application.Abstractions;
using Application.Abstractions.Services;
using Application.Examinations.Commands;
using Application.Examinations.Queries;
using Contracts.Examination;
using Domain.Entities;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/api/[controller]")]

public class ExaminationController : ControllerBase{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IFileStorageService _fileService;
    private readonly IExaminationRepository _examinationRepository;


    public ExaminationController(IMapper mapper, IMediator mediator, IFileStorageService fileStorageService, IExaminationRepository examinationRepository){
        _mediator = mediator;
        _mapper = mapper;
        _fileService = fileStorageService;
        _examinationRepository = examinationRepository;

    }
    [Authorize(Roles ="Admin")]
    [HttpPost("")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateExamination([FromForm] CreateExaminationRequest request){
        List<ExaminationResponse> examinationResponses = new();
        
        foreach(ExaminationRequest e in request.Examinations)
        {
            var examinationId = Guid.NewGuid();
            if(e.ImgDefault != null){
                
                var filepath = await _fileService.UploadExaminationImg(examinationId.ToString(), e.ImgDefault, null,e.ImgPath);
                var command  = new CreateExaminationCommand(
                    examinationId.ToString(),
                    e.Lab,
                    e.Name,
                    e.Type,
                    e.Area,
                    e.TextDefault,
                    filepath,
                    e.Cost
                );
                var examinationResult = await _mediator.Send(command);
                examinationResponses.Add(_mapper.Map<ExaminationResponse>(examinationResult));
            }
            else{
                var command  = new CreateExaminationCommand(
                    examinationId.ToString(),
                    e.Lab,
                    e.Name,
                    e.Type,
                    e.Area,
                    e.TextDefault,
                    null,
                    e.Cost
                );
                var examinationResult = await _mediator.Send(command);
                examinationResponses.Add(_mapper.Map<ExaminationResponse>(examinationResult));
            }

        }
        return Ok(examinationResponses);
    }

    [Authorize(Roles = "Admin,User")]
    [HttpGet()]
    public async Task<IActionResult> GetAllExaminations(){
        var examinationsResult = await _mediator.Send(new GetAllExaminations());
        var examinationResponses = examinationsResult.Select(e => _mapper.Map<ExaminationResponse>(e));
        return Ok(examinationResponses);
    }
    [Authorize(Roles ="Admin")]
    [HttpDelete("")]
    public async Task<IActionResult> DeleteExamination(List<DeleteExaminationRequest> request){
        var command = request.Select(r => _mapper.Map<DeleteExaminationCommand>(r));
        foreach (var c in command){
            await _mediator.Send(c);
        }
        return Ok(200);
    }
    [Authorize(Roles ="Admin")]
    [HttpPut("")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateExamination([FromForm] UpdateExaminationRequest request){
        //var commands = request.Examinations.Select(e => _mapper.Map<UpdateExaminationCommand>(e)).ToList();
        //List<QuestionExaminationResult> examinationResponse = new();
        List<ExaminationResponse> examinationResponses = new();
        
        foreach(ExaminationRequest e in request.Examinations)
        {
            var examination = await _examinationRepository.GetByIdAsync(new ExaminationId(new Guid(e.Id)));
            if(examination == null){
                throw new ArgumentException("This examination doesn't exist.");
            }
            if(e.ImgDefault != null){
                var filepath = await _fileService.UploadExaminationImg(e.Id, e.ImgDefault, null,e.ImgPath);
                var command  = new UpdateExaminationCommand(
                    new ExaminationId(new Guid(e.Id)),
                    e.Lab,
                    e.Name,
                    e.Type,
                    e.Area,
                    e.TextDefault,
                    e.Cost,
                    filepath
                );
                var examinationResult = await _mediator.Send(command);
                examinationResponses.Add(_mapper.Map<ExaminationResponse>(examinationResult));
            }
            else{
                if(e.ImgPath == null){
                    var oldImg = examination.ImgDefault;
                    if(oldImg != null){                
                        _fileService.DeleteFile(oldImg);
                    }
                }
                else{
                    if(!_fileService.CheckIfFilePathExist(e.ImgPath)){
                        throw new Exception ("Please enter correct image url.");
                    }
                }
                var command  = new UpdateExaminationCommand(
                    new ExaminationId(new Guid(e.Id)),
                    e.Lab,
                    e.Name,
                    e.Type,
                    e.Area,
                    e.TextDefault,
                    e.Cost,
                    e.ImgPath
                );
                var examinationResult = await _mediator.Send(command);
                examinationResponses.Add(_mapper.Map<ExaminationResponse>(examinationResult));
            }

        }
        
        return Ok(examinationResponses);
    }
}