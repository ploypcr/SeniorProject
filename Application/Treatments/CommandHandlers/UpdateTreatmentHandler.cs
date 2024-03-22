using Application.Abstractions;
using Application.Abstractions.Services;
using Application.Questions.Commands;
using Application.Treatments.Commands;
using Domain.Entities;
using MediatR;

namespace Application.Questions.CommandHandlers;

public class UpdateTreatmentHandler : IRequestHandler<UpdateTreatmentCommand, Treatment>
{
    private readonly ITreatmentRepository _treatmentRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IStatsRepository _statsRepository;

    private readonly IFileStorageService _fileService;
    public UpdateTreatmentHandler(IStatsRepository statsRepository,ITreatmentRepository treatmentRepository, IQuestionRepository questionRepository, IFileStorageService fileStorageService){
        _treatmentRepository = treatmentRepository;
        _fileService = fileStorageService;
        _questionRepository = questionRepository;
        _statsRepository = statsRepository;
    }

    public async Task<Treatment> Handle(UpdateTreatmentCommand request, CancellationToken cancellationToken)
    {
        var treatment = await _treatmentRepository.GetByIdAsync(request.TreatmentId);
        if(treatment == null){
            throw new ArgumentException("No treatment found.");
        }
        treatment.Update(
            treatment.Type, 
            request.Name,
            request.Cost);
        var questions = await _questionRepository.GetByTreatmentAsync(request.TreatmentId);

        foreach(Question q in questions){
            var studentStats = await _statsRepository.GetAllStudentStatsInQuestion(q.Id);
            foreach(var s in studentStats){
                await _statsRepository.DeleteStudentStats(s);
            }
            q.UpdateModified(true);
            await _questionRepository.UpdateQuestion(q);
        }

        await _treatmentRepository.UpdateAsync(treatment);
        return treatment;
    }
}
