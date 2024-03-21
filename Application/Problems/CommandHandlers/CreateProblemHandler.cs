using Application.Abstractions;
using Application.Problems.Commands;
using Application.Questions.Commands;
using Domain.Entities;
using MediatR;

namespace Application.Problems.CommandHandlers;

public class CreateProblemHandler : IRequestHandler<CreateProblemCommand, Problem>
{
    private readonly IProblemRepository _problemRepository;
    public CreateProblemHandler(IProblemRepository problemRepository){
        _problemRepository = problemRepository;
    }
    public async Task<Problem> Handle(CreateProblemCommand request, CancellationToken cancellationToken)
    {
        var problem = await _problemRepository.GetByNameAsync(request.Name);
        if(problem != null){
            throw new ArgumentException("Already has this problem.");
        }

        problem = Problem.Create(request.Name);

        await _problemRepository.AddAsync(problem);

        return problem;
    }
}