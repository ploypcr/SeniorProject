using Application.Abstractions;
using Domain.Entities;
using MediatR;

public class GetExaminationResultHandler : IRequestHandler<GetExaminationResultCommand, QuestionExamination>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IExaminationRepository _examinationRepository;

    public GetExaminationResultHandler(IQuestionRepository questionRepository, IExaminationRepository examinationRepository){
        _questionRepository = questionRepository;
        _examinationRepository = examinationRepository;
    }

    public async Task<QuestionExamination> Handle(GetExaminationResultCommand request, CancellationToken cancellationToken)
    {
        var examinations = await _examinationRepository.GetByIdAsync(new ExaminationId(new Guid(request.ExaminationId)));
        if(examinations == null){
            throw new ArgumentException("No examination");
        }
        return await _questionRepository.GetExaminationResult(new QuestionId(new Guid(request.QuestionId)), new ExaminationId(new Guid(request.ExaminationId)));
    }
}
