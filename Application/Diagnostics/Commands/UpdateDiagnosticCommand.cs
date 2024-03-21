using Domain.Entities;
using MediatR;

namespace Application.Diagnostics.Commands;

public record UpdateDiagnosticCommand(
    DiagnosticId DiagnosticId,
    string Name
) : IRequest<Diagnostic>;