using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Contracts.Student;
using Application.Student.Commands;
using Application.Student.Queries;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Contracts.Treatment;
using Contracts.Diagnostic;
namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,User")]
public class StudentController : ControllerBase{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    public StudentController(IMapper mapper, IMediator mediator){
        _mapper = mapper;
        _mediator = mediator;
    }
    [HttpPost("{questionId:guid}/stats")]
    public async Task<IActionResult> AddStudentStats(CreateStudentStatsRequest request, Guid questionId){
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var command = _mapper.Map<CreateStudentStatsCommand>((request, userId,questionId.ToString()));
        var createStudentStatResult = await _mediator.Send(command);
        return Ok(createStudentStatResult);
    } 
    [HttpGet("{questionId:guid}/stats")]
    public async Task<IActionResult> GetQuestionStudentStats(Guid questionId){
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var q = await _mediator.Send(new GetQuestionStatsInStudent(questionId.ToString(), userId));
        var getStudentStatsResponse = new QuestionStatsResponse(
                q.Question.Id.Value.ToString(),
                q.Question.Name.ToString(),
                q.Examination.Select(e => _mapper.Map<StudentExaminationResponse>(e)).ToList(),
                q.Problem.Select(e => _mapper.Map<StudentProblemResponse>(e)).ToList(),
                q.StudentStats.Treatments.Select(t => _mapper.Map<TreatmentResponse>(t)).ToList(),
                q.StudentStats.Diagnostics.Select(t => _mapper.Map<DiagnosticResponse>(t)).ToList(),
                q.StudentStats.Problem1_Score,
                q.StudentStats.Problem2_Score,
                q.StudentStats.Examination_Score,
                q.StudentStats.Treatment_Score,
                q.StudentStats.Diff_Diagnostic_Score,
                q.StudentStats.Ten_Diagnostic_Score,
                q.StudentStats.DateTime
            );
        return Ok(getStudentStatsResponse);
    } 

    [HttpGet("stats")]
    public async Task<IActionResult> GetAllQuestionStatsInStudent(){
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var getQuestionStatsResult = await _mediator.Send(new GetAllQuestionStatsInStudent(userId));

        List<QuestionStatsResponse> questionStatsResponses = new();
        foreach(var q in getQuestionStatsResult){
            questionStatsResponses.Add(new QuestionStatsResponse(
                q.Question.Id.Value.ToString(),
                q.Question.Name.ToString(),
                q.Examination.Select(e => _mapper.Map<StudentExaminationResponse>(e)).ToList(),
                q.Problem.Select(e => _mapper.Map<StudentProblemResponse>(e)).ToList(),
                q.StudentStats.Treatments.Select(t => _mapper.Map<TreatmentResponse>(t)).ToList(),
                q.StudentStats.Diagnostics.Select(t => _mapper.Map<DiagnosticResponse>(t)).ToList(),
                q.StudentStats.Problem1_Score,
                q.StudentStats.Problem2_Score,
                q.StudentStats.Examination_Score,
                q.StudentStats.Treatment_Score,
                q.StudentStats.Diff_Diagnostic_Score,
                q.StudentStats.Ten_Diagnostic_Score,
                q.StudentStats.DateTime
            ));
        }

        return Ok(questionStatsResponses);
    }
}