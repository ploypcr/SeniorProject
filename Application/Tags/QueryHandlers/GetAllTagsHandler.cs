using Application.Abstractions;
using Application.Questions.Queries;
using Application.Tags.Queries;
using Domain.Entities;
using MediatR;

namespace Application.Tags.QueryHandlers;
public class GetAllTagsHandler : IRequestHandler<GetAllTags, List<Tag>>
{
    private readonly ITagRepository _tagRepository;
    public GetAllTagsHandler(ITagRepository tagRepository){
        _tagRepository = tagRepository;
    }
    public async Task<List<Tag>> Handle(GetAllTags request, CancellationToken cancellationToken)
    {
        return await _tagRepository.GetAllTagsAsync();
    }
}