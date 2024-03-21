using Domain.Entities;
using MediatR;

namespace Application.Tags.Commands;
public record CreateTagCommand(
    string Name
) : IRequest<Tag>;