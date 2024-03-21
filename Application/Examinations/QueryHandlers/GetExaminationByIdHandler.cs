using Application.Abstractions;
using Application.Examinations.Queries;
using Domain.Entities;
using MediatR;

public class GetExaminationByIdHandler : IRequestHandler<GetExaminationById, Examination>
{
    private readonly IExaminationRepository _examinationRepository;
    public GetExaminationByIdHandler(IExaminationRepository examinationRepository){
        _examinationRepository = examinationRepository;
    }
    public async Task<Examination> Handle(GetExaminationById request, CancellationToken cancellationToken)
    {
        var examination = await _examinationRepository.GetByIdAsync(new ExaminationId(new Guid(request.Id)));
        
        return examination;
    }
}