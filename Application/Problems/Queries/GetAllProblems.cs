using Domain.Entities;
using MediatR;

namespace Application.Problems.Queries;

public record GetAllProblems(

) : IRequest<List<Problem>>;
