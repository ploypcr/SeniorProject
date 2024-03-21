using Domain.Entities;
using MediatR;

namespace Application.Problems.Commands;

public record CreateProblemCommand(
    string Name
): IRequest<Problem>;