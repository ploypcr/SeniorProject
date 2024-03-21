using Application.Abstractions;
using Application.Problems.Queries;
using Application.Questions.Queries;
using Domain.Entities;
using MediatR;

namespace Application.Problems.QueryHandlers;

public class GetAllProblemsHandler : IRequestHandler<GetAllProblems, List<Problem>>
{
    private readonly IProblemRepository _problemRepository;
    public GetAllProblemsHandler(IProblemRepository problemRepository){
        _problemRepository = problemRepository;
    }
    public async Task<List<Problem>> Handle(GetAllProblems request, CancellationToken cancellationToken)
    {
        return await _problemRepository.GetAllProblemsAsync();
    }
}
