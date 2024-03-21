using Application.Abstractions;
using Application.Questions.Queries;
using Domain.Entities;
using MediatR;

namespace Application.Questions.QueryHandlers;

public class GetQuestionByIdHandler : IRequestHandler<GetQuestionById, QuestionResult>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IProblemRepository _problemRepository;
    private readonly IExaminationRepository _examinatinRepository;
    private readonly IUserRepository _userRepository;

    public GetQuestionByIdHandler(IUserRepository userRepository,IQuestionRepository questionRepository, IProblemRepository problemRepository, IExaminationRepository examinationRepository)
    {
        _questionRepository = questionRepository;
        _problemRepository = problemRepository;
        _examinatinRepository = examinationRepository;
        _userRepository = userRepository;
    }

    public async Task<QuestionResult?> Handle(GetQuestionById request, CancellationToken cancellationToken)
    {
        var question = await _questionRepository.GetByIdAsync(request.QuestionId);
        if (question is null){
            throw new ArgumentException("Question not found.");
        }
        List<ProblemResult> problems = new ();
        foreach(QuestionProblem questionProblem in question.Problems){
            var problem = await _problemRepository.GetByIdAsync(questionProblem.ProblemId);
            problems.Add(new ProblemResult(problem.Id.Value, problem.Name, questionProblem.Round));
        } 
        List<ExaminationResult> examinations = new();
        foreach(QuestionExamination questionExamination in question.Examinations){{
            var examination = await _examinatinRepository.GetByIdAsync(questionExamination.ExaminationId);
            examinations.Add(new ExaminationResult(examination.Id.Value, examination.Lab,examination.Type, examination.Name, examination.Area, examination.Cost, questionExamination.TextResult ?? examination.TextDefault ?? "ค่าปกติ", questionExamination.ImgResult ?? examination.ImgDefault ?? null));
        }}
        
        List<LogResult> logs = new();
        foreach(QuestionLog questionLog in question.Logs){
            var user = await _userRepository.GetUserByIdAsync(questionLog.UserId);
            logs.Add(new LogResult($"{user.FirstName} {user.LastName}",questionLog.DateTime));
        }
        return new QuestionResult(question, problems, examinations, logs);
    }
}