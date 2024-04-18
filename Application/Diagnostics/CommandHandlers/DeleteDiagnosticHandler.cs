using Application.Abstractions;
using Application.Abstractions.Services;
using Application.Diagnostics.Commands;
using Application.Questions.Commands;
using Domain.Entities;
using MediatR;

namespace Application.Diagnostics.CommandHandlers;

public class DeleteDiagnosticHandler : IRequestHandler<DeleteDiagnosticCommand>
{
    private readonly IDiagnosticRepository _diagnosticRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IFileStorageService _fileService;
    private readonly IStatsRepository _statsRepository;

    public DeleteDiagnosticHandler(IStatsRepository statsRepository,IDiagnosticRepository diagnosticRepository, IQuestionRepository questionRepository, IFileStorageService fileStorageService){
        _diagnosticRepository = diagnosticRepository;
        _fileService = fileStorageService;
        _questionRepository = questionRepository;
        _statsRepository = statsRepository;
    }

    public async Task Handle(DeleteDiagnosticCommand request, CancellationToken cancellationToken)
    {
        var diagnostic = await _diagnosticRepository.GetByIdAsync(request.DiagnosticId);
        if(diagnostic == null){
            throw new ArgumentException("No Diagnostic found.");
        }
        var questions = await _questionRepository.GetByDiagnosticAsync(request.DiagnosticId);

        foreach(Question q in questions){
            q.UpdateModified(2);
            await _questionRepository.UpdateQuestion(q);
        }

        await _diagnosticRepository.DeleteAsync(diagnostic);

    }
}
