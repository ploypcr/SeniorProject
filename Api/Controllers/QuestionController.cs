using System.Drawing.Printing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Abstractions;
using Application.Abstractions.Services;
using Application.Authentication.Queries;
using Application.Examinations.Queries;
using Application.Questions.Commands;
using Application.Questions.Queries;
using Application.Student.Queries;
using Contracts.Diagnostic;
using Contracts.Examination;
using Contracts.Problem;
using Contracts.Question;
using Contracts.Student;
using Contracts.Treatment;
using Domain.Entities;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[ApiController]
[Route("/api/[controller]")]


public class QuestionController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IFileStorageService _fileService;
    private readonly IQuestionRepository _questionRepository;
    private readonly IExaminationRepository _examinationRepository;


    public QuestionController(IExaminationRepository examinationRepository,IMapper mapper, IMediator mediator, IFileStorageService fileStorageService, IQuestionRepository questionRepository)
    {
        _mapper = mapper;
        _mediator = mediator;
        _fileService = fileStorageService;
        _questionRepository = questionRepository;
        _examinationRepository = examinationRepository;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateQuestion([FromForm] CreateOrUpdateQuestionRequest request){
        List<ExaminationCommand> examinationCommands = new();

        if(request.Examinations != null){
            foreach(QuestionExaminationRequest e in request.Examinations)
            {
                var examination = await _examinationRepository.GetByIdAsync(new ExaminationId(new Guid(e.Id)));
                if(examination == null){
                    throw new ArgumentException("This examination doesn't exist");
                }
                if(e.ImgResult != null){
                    var filepath = await _fileService.UploadExaminationImg(e.Id, e.ImgResult, null,null);
                    examinationCommands.Add(new ExaminationCommand(e.Id,e.TextResult,filepath));
                }
                else{
                    examinationCommands.Add(new ExaminationCommand(e.Id,e.TextResult,null));
                }
            }
        }
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);;
        Console.WriteLine(userId);
        var command = _mapper.Map<CreateQuestionCommand>((request, userId, examinationCommands));
        var createQuestionResult = await _mediator.Send(command);
        return Ok(createQuestionResult);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateQuestion([FromForm] CreateOrUpdateQuestionRequest request, Guid id){
        List<ExaminationCommand> examinationCommands = new();
        var question = await _questionRepository.GetByIdAsync(new QuestionId(id));
        if (question == null){
            throw new ArgumentNullException("No question found.");
        }
        if(request.Examinations != null){
            foreach(QuestionExaminationRequest e in request.Examinations){
                var examination = await _examinationRepository.GetByIdAsync(new ExaminationId(new Guid(e.Id)));
                if(examination == null){
                    throw new ArgumentException("This examination doesn't exist");
                }
                if(e.ImgResult != null){
                    var filepath =  await _fileService.UploadExaminationImg(e.Id,e.ImgResult, null, e.ImgPath);
                    examinationCommands.Add(new ExaminationCommand(e.Id,e.TextResult,filepath));
                }
                else{
                    if(e.ImgPath == null){
                        var oldImg = question?.Examinations?.FirstOrDefault(qe => qe.ExaminationId.Value.ToString() == e.Id)?.ImgResult;
                        if(oldImg != null){                
                            _fileService.DeleteFile(oldImg);
                        }
                    }
                    else{
                        if(!_fileService.CheckIfFilePathExist(e.ImgPath)){
                            throw new Exception("Please enter correct image url.");
                        }
                    }
                    examinationCommands.Add(new ExaminationCommand(e.Id,e.TextResult,e.ImgPath));
                }
            }
        }
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var command = _mapper.Map<UpdateQuestionCommand>((request, userId, id.ToString(),examinationCommands));
        var updateQuestionResult = await _mediator.Send(command);
        return Ok(updateQuestionResult);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]

    public async Task<IActionResult> UploadQuestionFromExcelFile([FromForm] IFormFile file){
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // var command = _mapper.Map<CreateQuestionCommand>((request, userId));
        // var createQuestionResult = await _mediator.Send(command);
        List<CreateQuestionCommand> createQuestionCommands = await _fileService.ImportExcelFileToDB(file, userId);
        List<QuestionResponse> questionResponses = new();
        foreach(var c in createQuestionCommands){
            var createQuestionResult = await _mediator.Send(c);
            //questionResponses.Add(_mapper.Map<QuestionResponse>(createQuestionResult));
        }
        return Ok(200);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("template")]
    public async Task<IActionResult> GetExcelTemplate(){
        var fileBytes = await _fileService.GetExcelTemplate();
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "question_template.xlsx");
    }

    [HttpGet("")]
    [Authorize(Roles = "Admin,User")]

    public async Task<IActionResult> GetAllQuestions(List<string>? tagSearch){
        //Console.WriteLine(User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub).Value);
        var command = new GetAllQuestions(tagSearch);
        var getAllQuestionsResult = await _mediator.Send(command);
        var getOnlyPublishAndNoModifiedQuestion = getAllQuestionsResult.Where(q => q.Question.Status == 1 && q.Question.Modified == false);
        var result = getOnlyPublishAndNoModifiedQuestion.Select(q => _mapper.Map<QuestionResponse>(q)).ToList();
        return Ok(result);
    }

    [Authorize(Roles = "Admin,User")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetQuestionById(Guid id){
        //Console.WriteLine(User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub).Value);
        var command = new GetQuestionById(new QuestionId(id));
        var getQuestionsResult = await _mediator.Send(command);
        var result = _mapper.Map<QuestionResponse>(getQuestionsResult);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("solution")]
    public async Task<IActionResult> GetAllQuestionsWithSolution(List<string>? tagSearch){
        //Console.WriteLine(User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub).Value);
        var command = new GetAllQuestions(tagSearch);
        var getAllQuestionsResult = await _mediator.Send(command);
        var result = getAllQuestionsResult.Select(q => _mapper.Map<QuestionWithSolutionResponse>(q)).ToList();

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("solution/{id:guid}")]
    public async Task<IActionResult> GetQuestionWithSolutionById(Guid id){
        //Console.WriteLine(User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub).Value);
        var command = new GetQuestionById(new QuestionId(id));
        var getQuestionsResult = await _mediator.Send(command);
        var result =  _mapper.Map<QuestionWithSolutionResponse>(getQuestionsResult);
        return Ok(result);
    }

    [Authorize(Roles = "Admin,User")]
    [HttpGet("solution/examination/{id:guid}")]
    public async Task<IActionResult> GetQuestionExaminationSolution(Guid id){
        var getQuestionResult = await  _mediator.Send(new GetQuestionById(new QuestionId(id)));
        var examinations = getQuestionResult.Examinations;
        var examinationResponse = examinations.Select(e => _mapper.Map<ExaminationResponse>(e)).ToList();
        return Ok(examinationResponse);       
    }

    [Authorize(Roles = "Admin,User")]
    [HttpGet("solution/problem/{round}/{id:guid}")]
    public async Task<IActionResult> GetQuestionProblemSolution(Guid id, string round){
        var getQuestionResult = await  _mediator.Send(new GetQuestionById(new QuestionId(id)));
        var problems = getQuestionResult.Problems.Where(p => p.Round == Int32.Parse(round));
        var problemResponse = problems.Select(p => _mapper.Map<ProblemResponse>(p)).ToList();
        return Ok(problemResponse);         
    }

    [Authorize(Roles = "Admin,User")]
    [HttpGet("solution/diagnostic/{id:guid}")]
    public async Task<IActionResult> GetQuestionDiagnosticSolution(Guid id){
        var getQuestionResult = await  _mediator.Send(new GetQuestionById(new QuestionId(id)));
        var diagnostics = getQuestionResult.Question.Diagnostics;
        var diagnosticResponse = diagnostics.Select(d => _mapper.Map<DiagnosticResponse>(d)).ToList();
        return Ok(diagnosticResponse);           
    }

    [Authorize(Roles = "Admin,User")]
    [HttpGet("solution/treatment/{id:guid}")]
    public async Task<IActionResult> GetQuestionTreatmentSolution(Guid id){
        var getQuestionResult = await  _mediator.Send(new GetQuestionById(new QuestionId(id)));
        var treatments = getQuestionResult.Question.Treatments;
        var treatmentResponse = treatments.Select(t => _mapper.Map<TreatmentResponse>(t)).ToList();
        return Ok(treatmentResponse);         
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteQuestion(Guid id){

        await _mediator.Send(new DeleteQuestionCommand(new QuestionId(id)));
        return Ok(200);
    }

    [Authorize(Roles = "Admin,User")]
    [HttpPost("{id:guid}/examinationresult")]
    public async Task<IActionResult> GetExaminationResult(List<GetExaminationRequest> request, Guid id){
        var commands = request.Select(r => _mapper.Map<GetExaminationResultCommand>((r,id.ToString()))).ToList();
        List<QuestionExaminationResult> examinationResponse = new();
        foreach(var c in commands){
            var examinationResult = await _mediator.Send(c);
            var examination = await _examinationRepository.GetByIdAsync(new ExaminationId(new Guid(c.ExaminationId)));
            if(examination == null){
                throw new ArgumentException("This examination doesn't exist.");
            }
            if(examinationResult == null){
                examinationResponse.Add(new QuestionExaminationResult(
                    c.ExaminationId,
                    examination.TextDefault ?? "ค่าปกติ" ,
                    examination.ImgDefault ?? null
                ));
            }
            else{

                examinationResponse.Add(new QuestionExaminationResult(
                    c.ExaminationId,
                    examinationResult.TextResult == null ? ( examination.TextDefault == null ? "ค่าปกติ" : examination.TextDefault ) : examinationResult.TextResult,
                    examinationResult.ImgResult  == null ? (examination.ImgDefault == null ? null : examination.ImgDefault) : examinationResult.ImgResult
                ));
            }
        }
        return Ok(examinationResponse);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("{questionId:guid}/stats")]
    public async Task<IActionResult> GetAllStudentStatsInQuestion(Guid questionId){
        var getStudentStatsResult = await _mediator.Send(new GetAllStudentStatsInQuestion(questionId.ToString()));
        List<StudentStatsResponse> studentStatsResponses = new();
        foreach (var s in getStudentStatsResult){
            studentStatsResponses.Add(new StudentStatsResponse(
                s.Student.Id,
                s.Student.FirstName + " "+s.Student.LastName,
                s.Examination.Select(e => _mapper.Map<StudentExaminationResponse>(e)).ToList(),
                s.Problem.Select(e => _mapper.Map<StudentProblemResponse>(e)).ToList(),
                s.StudentStats.Treatments.Select(e => _mapper.Map<TreatmentResponse>(e)).ToList(),
                s.StudentStats.Diagnostics.Select(e => _mapper.Map<DiagnosticResponse>(e)).ToList(),
                s.StudentStats.Problem1_Score,
                s.StudentStats.Problem2_Score,
                s.StudentStats.Examination_Score,
                s.StudentStats.Treatment_Score,
                s.StudentStats.Diff_Diagnostic_Score,
                s.StudentStats.Ten_Diagnostic_Score,
                s.StudentStats.DateTime
                ));
        }
        //var studentStatsResponse = getStudentStatsResult.Select(s => _mapper.Map<StudentStatsResponse>(s)).ToList();
        return Ok(studentStatsResponses);
    }

}