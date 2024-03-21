using Domain.Entities;
using MediatR;


namespace Application.Problems.Commands;

public record UpdateProblemCommand(
    ProblemId ProblemId,
    string Name
) : IRequest<Problem>;