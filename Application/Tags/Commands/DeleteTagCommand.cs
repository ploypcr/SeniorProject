using Domain.Entities;
using MediatR;

namespace Application.Tags.Commands;
public record DeleteTagCommand(
    TagId TagId
) : IRequest;