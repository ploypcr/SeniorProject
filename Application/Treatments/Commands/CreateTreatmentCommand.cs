using Domain.Entities;
using MediatR;

namespace Application.Treatments.Commands;

public record CreateTreatmentCommand(
    string Name,
    string Type,
    int Cost
): IRequest<Treatment>;