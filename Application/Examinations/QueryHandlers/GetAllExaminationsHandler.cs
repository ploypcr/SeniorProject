using Application.Abstractions;
using Application.Examinations.Queries;
using Application.Questions.Queries;
using Domain.Entities;
using MediatR;

namespace Application.Examinations.QueryHandlers;

public class GetAllExaminationsHandler : IRequestHandler<GetAllExaminations, List<Examination>>
{
    private readonly IExaminationRepository _examinationRepository;
    public GetAllExaminationsHandler(IExaminationRepository examinationRepository){
        _examinationRepository = examinationRepository;
    }

    public async Task<List<Examination>> Handle(GetAllExaminations request, CancellationToken cancellationToken)
    {
        return await _examinationRepository.GetAllExaminationsAsync();
    }
}