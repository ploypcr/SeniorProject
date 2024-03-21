using Domain.Entities;
using MediatR;

namespace Application.Diagnostics.Commands;

public record CreateDiagnosticCommand(
    string Name,
    string Type
) : IRequest<Diagnostic>;