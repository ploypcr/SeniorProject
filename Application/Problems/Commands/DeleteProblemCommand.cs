using Domain.Entities;
using MediatR;


namespace Application.Problems.Commands;

public record DeleteProblemCommand(
    ProblemId ProblemId
) : IRequest;