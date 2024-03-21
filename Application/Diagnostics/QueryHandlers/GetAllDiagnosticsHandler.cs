using Application.Abstractions;
using Application.Diagnostics.Queries;
using Application.Questions.Queries;
using Domain.Entities;
using MediatR;

namespace Application.Diagnostics.QueryHandlers;

public class GetAllDiagnosticsHandler : IRequestHandler<GetAllDiagnostics, List<Diagnostic>>
{
    private readonly IDiagnosticRepository _diagnosticRepository;
    public GetAllDiagnosticsHandler(IDiagnosticRepository diagnosticRepository){
        _diagnosticRepository = diagnosticRepository;
    }
    public async Task<List<Diagnostic>> Handle(GetAllDiagnostics request, CancellationToken cancellationToken)
    {
        return await _diagnosticRepository.GetAllDiagnosticsAsync();
    }
}