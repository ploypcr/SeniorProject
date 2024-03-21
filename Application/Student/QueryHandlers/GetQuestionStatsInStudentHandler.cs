using Application.Abstractions;
using Application.Student.Queries;
using Domain.Entities;
using MediatR;

namespace Application.Student.QueryHandlers;

public class GetQuestionStatsHandler : IRequestHandler<GetQuestionStatsInStudent, QuestionStatsResult>
{
    private readonly IStatsRepository _statsRepository;
    private readonly IExaminationRepository _examinationRepository;
    private readonly IProblemRepository _problemRepository;
    private readonly IQuestionRepository _questionRepository;

    public GetQuestionStatsHandler(IQuestionRepository questionRepository,IStatsRepository statsRepository, IExaminationRepository examinationRepository, IProblemRepository problemRepository){
        _statsRepository = statsRepository;
        _examinationRepository = examinationRepository;
        _problemRepository = problemRepository;
        _questionRepository = questionRepository;
    }

    public async Task<QuestionStatsResult> Handle(GetQuestionStatsInStudent request, CancellationToken cancellationToken)
    {
        var question = await _questionRepository.GetByIdAsync(new QuestionId(new Guid(request.QuestionId)));
        if (question == null){
            throw new ArgumentException("This question doesn't exist.");
        }
        var studentStats = await _statsRepository.GetStudentStats(request.UserId, new QuestionId(new  Guid(request.QuestionId)));
        if(studentStats == null){
            throw new ArgumentException("This student hasn't done this question yet.");
        }
        List<StudentExaminationsResult> examinationsResults = new();
        List<StudentProblemsResult> problemsResults = new();
        foreach(var e in studentStats.Examinations){
            var examination = await _examinationRepository.GetByIdAsync(e.ExaminationId);
            examinationsResults.Add(new StudentExaminationsResult(examination.Id.Value.ToString(),examination.Name, examination.Type,examination.Lab, examination.Area,examination.Cost));
        }
        foreach(var p in studentStats.Problems){
            var problem = await _problemRepository.GetByIdAsync(p.ProblemId);
            problemsResults.Add(new StudentProblemsResult(problem.Id.Value.ToString(),problem.Name, p.Round));
        }
        return new QuestionStatsResult(question, studentStats, examinationsResults, problemsResults);
    }
}

