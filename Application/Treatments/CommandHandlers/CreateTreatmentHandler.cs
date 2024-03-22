using Application.Abstractions;
using Application.Questions.Commands;
using Application.Treatments.Commands;
using Domain.Entities;
using MediatR;

namespace Application.Questions.CommandHandlers;

public class CreateTreatmentHandler : IRequestHandler<CreateTreatmentCommand, Treatment>
{
    private readonly ITreatmentRepository _treatmentRepository;
    public CreateTreatmentHandler(ITreatmentRepository treatmentRepository){
        _treatmentRepository = treatmentRepository;
    }

    public async Task<Treatment> Handle(CreateTreatmentCommand request, CancellationToken cancellationToken)
    {
        var treatment = await _treatmentRepository.GetByNameAndTypeAsync(request.Name, request.Type);
        if(treatment != null){
            throw new ArgumentException("Already has this treatment.");
        }

        treatment = Treatment.Create(request.Type, request.Name, request.Cost);
        await _treatmentRepository.AddAsync(treatment);
        return treatment;
    }
}