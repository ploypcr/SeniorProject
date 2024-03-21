using Domain.Entities;
using MediatR;

namespace Application.Treatments.Commands;
public record DeleteTreatmentCommand(
    TreatmentId TreatmentId
) : IRequest;