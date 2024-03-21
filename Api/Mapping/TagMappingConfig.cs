using Application.Tags.Commands;
using Contracts.Tag;
using Domain.Entities;
using Mapster;

namespace Api.Mapping;

public class TagMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Tag, TagResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest, src => src);
        config.NewConfig<CreateTagRequest, CreateTagCommand>()
            .Map(dest => dest, src => src);
        config.NewConfig<UpdateTagRequest, UpdateTagCommand>()
            .Map(dest => dest.TagId, src => new TagId(new Guid(src.Id)))
            .Map(dest => dest, src => src);

        config.NewConfig<DeleteTagRequest, DeleteTagCommand>()
            .Map(dest => dest.TagId, src => new TagId(new Guid(src.Id)));
    }
}
