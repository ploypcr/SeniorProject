using System.Reflection.PortableExecutable;
using System.Net;
using System.Net.Cache;
using Application.Abstractions;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Questions.Commands;

public class CreateQuestionHandler : IRequestHandler<CreateQuestionCommand, Question>{
    private readonly IQuestionRepository _questionRepository;
    private readonly IProblemRepository _problemRepository;
    private readonly IDiagnosticRepository _diagnosticRepository;
    private readonly ITreatmentRepository _treatmentRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IExaminationRepository _examinationRepository;
    public CreateQuestionHandler(IQuestionRepository questionRepository, IProblemRepository problemRepository, IDiagnosticRepository diagnosticRepository, ITreatmentRepository treatmentRepository, ITagRepository tagRepository, IExaminationRepository examinationRepository)
    {
        _questionRepository = questionRepository;
        _problemRepository = problemRepository;
        _diagnosticRepository = diagnosticRepository;
        _treatmentRepository = treatmentRepository;
        _tagRepository = tagRepository;
        _examinationRepository = examinationRepository;
    }

    public async Task<Question> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        // if(request.ClientComplains == null){
        //     Console.WriteLine("clientcomplains is null");
        // }
        // Console.WriteLine(request.ClientComplains == null ? "yes":"no", request.HistoryTakingInfo == null ? "yes":"no", request.GeneralInfo == null ? "yes":"no", request.Signalment == null ? "yes":"no");
        var name = await _questionRepository.GetLastestName(); 
        var question = Question.Create(
            name+1,
            request.ClientComplains,
            request.HistoryTakingInfo,
            request.GeneralInfo,
            new Signalment(
                request.Signalment?.Species,
                request.Signalment?.Breed,
                request.Signalment?.Gender,
                request.Signalment?.Sterilize,
                request.Signalment?.Age,
                request.Signalment?.Weight
                ),//request.Signalment?.Age,
                //request.Signalment?.Weight),
            request.Status,
            request.ExtraQues
        );

        question.AddUser(request.UserId);
        if(request.Problems != null){
            foreach(ProblemCommand pb in request.Problems){
                var problem = await _problemRepository.GetByIdAsync(new ProblemId(new Guid(pb.Id)));
                if (problem == null){
                    throw new ArgumentException("No problem found.");
                }
                Console.WriteLine(problem.Id);
                question.AddProblem(problem.Id, pb.Round);
            }
        }

        if(request.Examinations != null){
            foreach (ExaminationCommand e in request.Examinations){
                var examination = await _examinationRepository.GetByIdAsync(new ExaminationId(new Guid(e.Id)));
                if(examination == null){
                    throw new ArgumentException("No Examination found.");
                }
                question.AddExamination(
                    new ExaminationId(new Guid(e.Id)),
                    e.TextResult,
                    e.ImgResult);
            }
        }

        if(request.Treatments != null){
            foreach (TreatmentCommand t in request.Treatments){
                var treatment = await _treatmentRepository.GetByIdAsync(new TreatmentId(new Guid(t.Id)));
                if (treatment == null){
                    throw new ArgumentException("No treatment found");
                }
                question.AddTreatment(treatment);
            }
        }

        if(request.Diagnostics != null){
            foreach (DiagnosticCommand d in request.Diagnostics){
                var diagnostic = await _diagnosticRepository.GetByIdAsync(new DiagnosticId(new Guid(d.Id)));
                if (diagnostic == null){
                    throw new ArgumentException("No Diagnostic Found");
                }
                question.AddDiagnostic(diagnostic);
            }
        }
        if(request.Tags != null){
            foreach (TagCommand t in request.Tags){
                var tag = await _tagRepository.GetByIdAsync(new TagId(new Guid(t.Id)));
                if (tag == null){
                    throw new ArgumentException("No Tag found");
                }
                question.AddTag(tag);
            }
        }

        return await _questionRepository.AddQuestion(question);
    }
};