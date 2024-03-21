using Domain.Entities;
using MediatR;

namespace Application.Diagnostics.Queries;
public record GetAllDiagnostics(

) : IRequest<List<Diagnostic>>;