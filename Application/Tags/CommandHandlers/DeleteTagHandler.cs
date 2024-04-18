using Application.Abstractions;
using Application.Abstractions.Services;
using Application.Questions.Commands;
using Application.Tags.Commands;
using Domain.Entities;
using MediatR;

namespace Application.Tags.CommandHandlers;


public class DeleteTagHandler : IRequestHandler<DeleteTagCommand>
{
    private readonly ITagRepository _tagRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IFileStorageService _fileService;
    private readonly IStatsRepository _statsRepository;

    public DeleteTagHandler(IStatsRepository statsRepository,ITagRepository tagRepository, IQuestionRepository questionRepository, IFileStorageService fileStorageService){
        _tagRepository = tagRepository;
        _fileService = fileStorageService;
        _questionRepository = questionRepository;
        _statsRepository=statsRepository;
    }

    public async Task Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.GetByIdAsync(request.TagId);
        if(tag == null){
            throw new ArgumentException("No tag found.");
        }
        var questions = await _questionRepository.GetByTagAsync(request.TagId);

        foreach(Question q in questions){
            q.UpdateModified(2);
            await _questionRepository.UpdateQuestion(q);
        }

        await _tagRepository.DeleteAsync(tag);

    }
}
