using Application.Abstractions;
using Application.Examinations.Commands;
using Application.Questions.Commands;
using Domain.Entities;
using MediatR;

namespace Application.Examinations.CommandHandlers;

public class CreateExaminationHandler : IRequestHandler<CreateExaminationCommand, Examination>
{
    private readonly IExaminationRepository _examinationRepository;
    public CreateExaminationHandler(IExaminationRepository examinationRepository){
        _examinationRepository = examinationRepository;
    }

    public async Task<Examination> Handle(CreateExaminationCommand request, CancellationToken cancellationToken)
    {
        var examination = await _examinationRepository.GetByDetails(request.Name, request.Type, request.Lab, request.Area);
        if(examination != null){
            throw new ArgumentException("Already has this examination.");
        }

        examination = Examination.Create(
            request.Id,
            request.Lab,
            request.Name, 
            request.Type, 
            request.Area,
            request.Cost,
            request.TextDefault,
            request.ImgDefault);

        await _examinationRepository.AddAsync(examination);

        return examination;
    }
}