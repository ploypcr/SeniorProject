using Domain.Entities;
using MediatR;

namespace Application.Tags.Commands;
public record UpdateTagCommand(
    TagId TagId,
    string Name
) : IRequest<Tag>;