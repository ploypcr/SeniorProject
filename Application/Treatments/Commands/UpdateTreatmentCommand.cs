using Domain.Entities;
using MediatR;

namespace Application.Treatments.Commands;

public record UpdateTreatmentCommand(
    TreatmentId TreatmentId,
    string Name,
    string Type,
    int Cost
) : IRequest<Treatment>;