using Application.Abstractions;
using Application.Abstractions.Services;
using Application.Questions.Commands;
using Application.Treatments.Commands;
using Domain.Entities;
using MediatR;

namespace Application.Questions.CommandHandlers;

public class DeleteTreatmentHandler : IRequestHandler<DeleteTreatmentCommand>
{
    private readonly ITreatmentRepository _treatmentRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IFileStorageService _fileService;
    private readonly IStatsRepository _statsRepository;

    public DeleteTreatmentHandler(IStatsRepository statsRepository,ITreatmentRepository treatmentRepository, IQuestionRepository questionRepository, IFileStorageService fileStorageService){
        _treatmentRepository = treatmentRepository;
        _fileService = fileStorageService;
        _questionRepository = questionRepository;
        _statsRepository = statsRepository;
    }

    public async Task Handle(DeleteTreatmentCommand request, CancellationToken cancellationToken)
    {
        var treatment = await _treatmentRepository.GetByIdAsync(request.TreatmentId);
        if(treatment == null){
            throw new ArgumentException("No treatment found.");
        }
        var questions = await _questionRepository.GetByTreatmentAsync(request.TreatmentId);

        foreach(Question q in questions){
            q.UpdateModified(2);
            await _questionRepository.UpdateQuestion(q);
        }

        await _treatmentRepository.DeleteAsync(treatment);

    }
}
