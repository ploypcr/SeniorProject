using Application.Abstractions;
using Application.Questions.Commands;
using Domain.Entities;
using MediatR;

namespace Application.Questions.CommandHandlers;

public class UpdateQuestionHandler : IRequestHandler<UpdateQuestionCommand, Question>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IProblemRepository _problemRepository;
    private readonly IExaminationRepository _examinationRepository;
    private readonly ITreatmentRepository _treatmentRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IDiagnosticRepository _diagnosticRepository;
    private readonly IUserRepository _userRepository;
    private readonly IStatsRepository _statsRepository;



    public UpdateQuestionHandler(IStatsRepository statsRepository,IUserRepository userRepository,IQuestionRepository questionRepository, IProblemRepository problemRepository, IExaminationRepository examinationRepository, ITreatmentRepository treatmentRepository, ITagRepository tagRepository, IDiagnosticRepository diagnosticRepository){
        _questionRepository = questionRepository;
        _problemRepository = problemRepository;
        _examinationRepository = examinationRepository;
        _treatmentRepository = treatmentRepository;
        _tagRepository = tagRepository;
        _diagnosticRepository = diagnosticRepository;
        _userRepository = userRepository;
        _statsRepository = statsRepository;
    }
    public async Task<Question> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _questionRepository.GetByIdAsync(new QuestionId(new Guid(request.Id)));
        if(question == null){
            throw new ArgumentException("Don't have this question.");
        }

        var name = question.Name;
        // if(request.Status != question.Status){
        //     name = await _questionRepository.GetLastestName()+1;
        // }
        question.UpdateQuestion(
                name,
                request.ClientComplains, 
                request.HistoryTakingInfo, 
                request.GeneralInfo, 
                new Signalment(
                    request?.Signalment?.Species, 
                    request?.Signalment?.Breed, 
                    request?.Signalment?.Gender, 
                    request?.Signalment?.Sterilize, 
                    request?.Signalment?.Age,
                     request?.Signalment?.Weight),
                request.Status,
                request.ExtraQues
            );

        // check if old problem in question doesn't exist in request and remove it.
        List<ProblemCommand> qp = new();
        foreach(QuestionProblem questionProblem in question.Problems){
            if(request.Problems != null){
                if(!request.Problems.Any(rp => rp.Equals(new ProblemCommand(questionProblem.ProblemId.Value.ToString(), questionProblem.Round)))){
                    question.RemoveProblem(questionProblem);
                }
                else{
                    qp.Add(new ProblemCommand(questionProblem.ProblemId.Value.ToString(), questionProblem.Round));
                }
            }else{
                question.RemoveProblem(questionProblem);
            }
        }

        // check if request problem doesn't exist in question and add it.
        if(request.Problems != null){
            foreach(ProblemCommand problemRequest in request.Problems){
                var problem = await _problemRepository.GetByIdAsync(new ProblemId(new Guid(problemRequest.Id)));
                if(problem == null){
                    throw new ArgumentException("This problem doesn't exist.");
                }
                if(!qp.Any(rp => rp.Equals(new ProblemCommand(problemRequest.Id, problemRequest.Round)))){
                    question.AddProblem(new ProblemId(new Guid(problemRequest.Id)), problemRequest.Round);
                }
            }
        }

        List<ExaminationCommand> qe = new();
        foreach (QuestionExamination questionExamination in question.Examinations){
            var examination = await _examinationRepository.GetByIdAsync(questionExamination.ExaminationId);
            if(request.Examinations != null){
                if(!request.Examinations.Any(rp => rp.Equals(new ExaminationCommand(
                    examination.Id.Value.ToString(), 
                    questionExamination.TextResult, 
                    questionExamination.ImgResult
                    )))){
                    question.RemoveExamination(questionExamination);
                }
                else{
                    qe.Add(new ExaminationCommand(examination.Id.Value.ToString(), questionExamination.TextResult, questionExamination.ImgResult));
                }
            }else{
                question.RemoveExamination(questionExamination);
            }
        }

        if(request.Examinations != null){
            foreach(ExaminationCommand examinationRequest in request.Examinations){
                var examination = await _examinationRepository.GetByIdAsync(new ExaminationId(new Guid(examinationRequest.Id)));
                if(examination == null){
                    throw new ArgumentException("This examination doesn't exist");
                }
                if(!qe.Any(re => re.Equals(examinationRequest))){
                    question.AddExamination(new ExaminationId(new Guid(examinationRequest.Id)), examinationRequest.TextResult, examinationRequest.ImgResult);
                }
            }
        }

        List<TreatmentCommand> qt = new();
        foreach (Treatment t in question.Treatments){
            var treatmentCommand = new TreatmentCommand(t.Id.Value.ToString());
            if(request.Treatments != null){
                if(!request.Treatments.Any(rp => rp.Equals(treatmentCommand))){
                    question.RemoveTreatment(t);
                }
                else{
                    qt.Add(treatmentCommand);
                }
            }else{
                question.RemoveTreatment(t);
            }
        }

        if(request.Treatments != null){
            foreach (TreatmentCommand treatmentRequest in request.Treatments){
                var treatment = await _treatmentRepository.GetByIdAsync(new TreatmentId(new Guid(treatmentRequest.Id)));
                if(treatment == null){
                    throw new ArgumentException("This treatment doesn't exist.");
                }
                if(!qt.Any(rt => rt.Equals(treatmentRequest))){
                    question.AddTreatment(treatment);
                }
            }
        }

        List<DiagnosticCommand> qd = new();
        foreach (Diagnostic d in question.Diagnostics){
            var diagnosticCommand = new DiagnosticCommand(d.Id.Value.ToString());
            if(request.Diagnostics != null){
                if(!request.Diagnostics.Any(r => r.Equals(diagnosticCommand))){
                    question.RemoveDiagnostic(d);
                }
                else{
                    qd.Add(diagnosticCommand);
                }
            }else{
                question.RemoveDiagnostic(d);
            }
        }
        if(request.Diagnostics != null){
            foreach (DiagnosticCommand diagnosticRequest in request.Diagnostics){
                var diagnostic = await _diagnosticRepository.GetByIdAsync(new DiagnosticId(new Guid(diagnosticRequest.Id)));
                if(diagnostic == null){
                        throw new ArgumentException("This diagnosis doesn't exist.");
                }
                if(!qd.Any(q => q.Equals(diagnosticRequest))){
                    question.AddDiagnostic(diagnostic);
                }
            }
        }

        List<TagCommand> qtg = new();
        foreach (Tag t in question.Tags){
            var tagCommand = new TagCommand(t.Id.Value.ToString());
            if(request.Tags != null){
                if(!request.Tags.Any(r => r.Equals(tagCommand))){
                    question.RemoveTag(t);
                }
                else{
                    qtg.Add(tagCommand);
                }
            }else{
                question.RemoveTag(t);
            }
        }

        if(request.Tags != null){
            foreach (TagCommand tagRequest in request.Tags){
                var tag = await _tagRepository.GetByIdAsync(new TagId(new Guid(tagRequest.Id)));
                    if(tag == null){
                        throw new ArgumentException("This tag doesn't exist");
                }
                if(!qtg.Any(q => q.Equals(tagRequest))){
                    question.AddTag(tag);
                }
            }
        }

        var user = await _userRepository.GetUserByIdAsync(request.UserId);
        if(user == null || user.FirstName != "Admin"){
            throw new ArgumentException("Invalid user.");
        }
        
        question.AddUser(request.UserId);

        if(question.Modified != 0){
            question.UpdateModified(0);
        }
        await _questionRepository.UpdateQuestion(question);
        return question;
    }
}