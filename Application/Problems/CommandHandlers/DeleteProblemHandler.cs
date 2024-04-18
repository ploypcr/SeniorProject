using Application.Abstractions;
using Application.Abstractions.Services;
using Application.Problems.Commands;
using Application.Questions.Commands;
using Domain.Entities;
using MediatR;

namespace Application.Problems.CommandHandlers;
public class DeleteProblemHandler : IRequestHandler<DeleteProblemCommand>
{
    private readonly IProblemRepository _problemRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IFileStorageService _fileService;
    private readonly IStatsRepository _statsRepository;

    public DeleteProblemHandler(IStatsRepository statsRepository,IProblemRepository problemRepository, IQuestionRepository questionRepository, IFileStorageService fileStorageService){
        _problemRepository = problemRepository;
        _fileService = fileStorageService;
        _questionRepository = questionRepository;
        _statsRepository = statsRepository;
    }

    public async Task Handle(DeleteProblemCommand request, CancellationToken cancellationToken)
    {
        var problem = await _problemRepository.GetByIdAsync(request.ProblemId);
        if(problem == null){
            throw new ArgumentException("No problem found.");
        }
        var questions = await _questionRepository.GetByProblemAsync(request.ProblemId);

        foreach(Question q in questions){
            q.UpdateModified(2);
            await _questionRepository.UpdateQuestion(q);
        }

        await _problemRepository.DeleteAsync(problem);

    }
}
