using Domain.Entities;
using MediatR;

namespace Application.Diagnostics.Commands;

public record DeleteDiagnosticCommand(
    DiagnosticId DiagnosticId
) : IRequest;