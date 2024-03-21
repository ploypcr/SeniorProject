using Application.Abstractions;
using Application.Questions.Commands;
using Application.Tags.Commands;
using Domain.Entities;
using MediatR;

namespace Application.Tags.CommandHandlers;

public class CreateTagHandler : IRequestHandler<CreateTagCommand, Tag>
{
    private readonly ITagRepository _tagRepository;
    public CreateTagHandler(ITagRepository tagRepository){
        _tagRepository = tagRepository;
    }
    public async Task<Tag> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.GetByNameAsync(request.Name);
        if(tag != null){
            throw new ArgumentException("Already has this tag.");
        }
        
        tag = Tag.Create(request.Name);
        await _tagRepository.AddAsync(tag);
        return tag;
    }
}
