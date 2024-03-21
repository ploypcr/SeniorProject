using Application.Abstractions;
using Application.Questions.Queries;
using Domain.Entities;
using MediatR;

public class GetAllQuestionsHandler : IRequestHandler<GetAllQuestions, List<QuestionResult>>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IProblemRepository _problemRepository;
    private readonly IExaminationRepository  _examinationRepository;
    private readonly IUserRepository  _userRepository;

    public GetAllQuestionsHandler(IUserRepository userRepository,IQuestionRepository questionRepository, IProblemRepository problemRepository, IExaminationRepository examinationRepository){
        _questionRepository = questionRepository;
        _problemRepository = problemRepository;
        _examinationRepository = examinationRepository;
        _userRepository = userRepository;
    }

    public async Task<List<QuestionResult>> Handle(GetAllQuestions request, CancellationToken cancellationToken)
    {
        List<QuestionResult> questionResults = new List<QuestionResult>();
        var questions = await _questionRepository.GetAllQuestionsAsync(request.TagSearch);
        foreach(Question question in questions){

            List<ProblemResult> problemResults = new();
            foreach(QuestionProblem questionProblem in question.Problems){
                var problems = await _problemRepository.GetByIdAsync(questionProblem.ProblemId);
                problemResults.Add(new ProblemResult(problems.Id.Value, problems.Name, questionProblem.Round));
            }

            List<ExaminationResult> examinationResults = new();
            foreach(QuestionExamination questionExamination in question.Examinations){
                var examinations = await _examinationRepository.GetByIdAsync(questionExamination.ExaminationId);
                examinationResults.Add(new ExaminationResult(examinations.Id.Value, examinations.Lab,examinations.Type, examinations.Name, examinations.Area, examinations.Cost, questionExamination.TextResult ?? examinations.TextDefault ?? "ค่าปกติ", questionExamination.ImgResult ?? examinations.ImgDefault ?? null));
                
            }
            List<LogResult> logs = new();
            foreach(QuestionLog questionLog in question.Logs){
                var user = await _userRepository.GetUserByIdAsync(questionLog.UserId);
                logs.Add(new LogResult($"{user.FirstName} {user.LastName}",questionLog.DateTime));
            }
            questionResults.Add(new QuestionResult(question, problemResults,examinationResults, logs));
        }
        return questionResults;
    }
}