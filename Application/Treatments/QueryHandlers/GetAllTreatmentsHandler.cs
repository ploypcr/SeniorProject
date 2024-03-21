using Application.Abstractions;
using Application.Questions.Queries;
using Application.Treatments.Queries;
using Domain.Entities;
using MediatR;
namespace Application.Treatments.QueryHandlers;

public class GetAllTreatmentsHandlers : IRequestHandler<GetAllTreatments, List<Treatment>>
{
    private readonly ITreatmentRepository _treatmentRepository;
    public GetAllTreatmentsHandlers(ITreatmentRepository treatmentRepository){
        _treatmentRepository = treatmentRepository;
    }
    public async Task<List<Treatment>> Handle(GetAllTreatments request, CancellationToken cancellationToken)
    {
        return await _treatmentRepository.GetAllTreatmentAsync();
    }
}