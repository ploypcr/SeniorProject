using Application.Abstractions;
using Application.Student.Queries;
using Domain.Entities;
using MediatR;

namespace Application.Student.QueryHandlers;

public class GetAllQuestionStatsHandler : IRequestHandler<GetAllQuestionStatsInStudent, List<QuestionStatsResult>>
{
    private readonly IStatsRepository _statsRepository;
    private readonly IExaminationRepository _examinationRepository;
    private readonly IProblemRepository _problemRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IUserRepository _userRepository;


    public GetAllQuestionStatsHandler(IUserRepository userRepository,IQuestionRepository questionRepository,IStatsRepository statsRepository, IExaminationRepository examinationRepository, IProblemRepository problemRepository){
        _statsRepository = statsRepository;
        _examinationRepository = examinationRepository;
        _problemRepository = problemRepository;
        _questionRepository = questionRepository;
        _userRepository = userRepository;
    }
    public async Task<List<QuestionStatsResult>> Handle(GetAllQuestionStatsInStudent request, CancellationToken cancellationToken)
    {
        List<QuestionStatsResult> questionStatsResults = new();
        var user = await _userRepository.GetUserByIdAsync(request.UserId);
        if(user == null){
            throw new ArgumentException("This User doesn't exist");
        }
        var studentStats = await _statsRepository.GetAllQuestionStatsInStudent(request.UserId);
        foreach(var s in studentStats){
            //Console.WriteLine(s.Problem1_Score);
            List<StudentExaminationsResult> examinationsResults = new();
            List<StudentProblemsResult> problemsResults = new();
            var question = await _questionRepository.GetByIdAsync(s.QuestionId);
            foreach(var e in s.Examinations){
                var examination = await _examinationRepository.GetByIdAsync(e.ExaminationId);
                examinationsResults.Add(new StudentExaminationsResult(examination.Id.Value.ToString(),examination.Name, examination.Type,examination.Lab, examination.Area,examination.Cost));
            }
            foreach(var p in s.Problems){
                var problem = await _problemRepository.GetByIdAsync(p.ProblemId);
                problemsResults.Add(new StudentProblemsResult(problem.Id.Value.ToString(),problem.Name, p.Round));
            }
            questionStatsResults.Add(new QuestionStatsResult(question, s,examinationsResults,problemsResults));
        }
        return questionStatsResults;
    }
    
}

