using Application.Abstractions;
using Application.Student.Queries;
using Domain.Entities;
using MediatR;

namespace Application.Student.QueryHandlers;

public class GetAllStudentStatsHandler : IRequestHandler<GetAllStudentStatsInQuestion, List<StudentStatsResult>>
{
    private readonly IStatsRepository _statsRepository;
    private readonly IExaminationRepository _examinationRepository;
    private readonly IProblemRepository _problemRepository;
    private readonly IQuestionRepository _questionRepository;

    private readonly IUserRepository _userRepository;

    public GetAllStudentStatsHandler(IQuestionRepository questionRepository,IUserRepository userRepository,IStatsRepository statsRepository, IExaminationRepository examinationRepository, IProblemRepository problemRepository){
        _statsRepository = statsRepository;
        _examinationRepository = examinationRepository;
        _problemRepository = problemRepository;
        _userRepository = userRepository;
        _questionRepository = questionRepository;
    }
    public async Task<List<StudentStatsResult>> Handle(GetAllStudentStatsInQuestion request, CancellationToken cancellationToken)
    {
        List<StudentStatsResult> studentStatsResults = new();

        var question = await _questionRepository.GetByIdAsync(new QuestionId(new Guid(request.QuestionId)));
        if(question == null){
            throw new ArgumentException("This question doesn't exist.");
        }
        var studentStats = await _statsRepository.GetAllStudentStatsInQuestion(new QuestionId(new Guid(request.QuestionId)));
        foreach(var s in studentStats){
            List<StudentExaminationsResult> examinationsResults = new();
            List<StudentProblemsResult> problemsResults = new();
            var student = await _userRepository.GetUserByIdAsync(s.UserId.ToString());
            foreach(var e in s.Examinations){
                var examination = await _examinationRepository.GetByIdAsync(e.ExaminationId);
                examinationsResults.Add(new StudentExaminationsResult(examination.Id.Value.ToString(),examination.Name, examination.Type,examination.Lab, examination.Area,examination.Cost));
            }
            foreach(var p in s.Problems){
                var problem = await _problemRepository.GetByIdAsync(p.ProblemId);
                problemsResults.Add(new StudentProblemsResult(problem.Id.Value.ToString(),problem.Name, p.Round));
            }
            studentStatsResults.Add(new StudentStatsResult(student, s,examinationsResults,problemsResults));
        }
        return studentStatsResults;
    }
    
}