using Domain.Entities;
using MediatR;

namespace Application.Tags.Queries;
public record GetAllTags(

) : IRequest<List<Tag>>;