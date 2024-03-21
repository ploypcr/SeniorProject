using Application.Abstractions;
using Application.Diagnostics.Commands;
using Application.Questions.Commands;
using Domain.Entities;
using MediatR;

namespace Application.Diagnostics.CommandHandlers;

public class CreateDiagnosticHandler : IRequestHandler<CreateDiagnosticCommand, Diagnostic>
{
    private readonly IDiagnosticRepository _diagnosticRepository;
    public CreateDiagnosticHandler(IDiagnosticRepository diagnosticRepository){
        _diagnosticRepository = diagnosticRepository;
    }
    public async Task<Diagnostic> Handle(CreateDiagnosticCommand request, CancellationToken cancellationToken)
    {
        var diagnostic = await _diagnosticRepository.GetByNameAndTypeAsync(request.Name, request.Type);
        if(diagnostic != null){
            throw new ArgumentException("Already has this diagnostic.");
        }

        diagnostic = Diagnostic.Create(request.Name, request.Type);

        await _diagnosticRepository.AddAsync(diagnostic);
        return diagnostic;
    }
}
