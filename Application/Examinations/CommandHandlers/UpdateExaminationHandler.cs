using Application.Abstractions;
using Application.Abstractions.Services;
using Application.Examinations.Commands;
using Application.Questions.Commands;
using Domain.Entities;
using MediatR;

namespace Application.Examinations.CommandHandlers;

public class UpdateExaminationHandler : IRequestHandler<UpdateExaminationCommand, Examination>
{
    private readonly IExaminationRepository _examinationRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IFileStorageService _fileService;
    private readonly IStatsRepository _statsRepository;

    public UpdateExaminationHandler(IStatsRepository statsRepository,IExaminationRepository examinationRepository, IQuestionRepository questionRepository, IFileStorageService fileStorageService){
        _examinationRepository = examinationRepository;
        _fileService = fileStorageService;
        _questionRepository = questionRepository;
        _statsRepository = statsRepository;
    }

    public async Task<Examination> Handle(UpdateExaminationCommand request, CancellationToken cancellationToken)
    {
        var examination = await _examinationRepository.GetByIdAsync(request.ExaminationId);
        if(examination == null){
            throw new ArgumentException("No examination found.");
        }
        examination.Update(
            request.Lab, 
            request.Name, 
            request.Type, 
            request.Area,
            request.Cost,
            request.TextDefault,
            request.ImgDefault);
            
        var questions = await _questionRepository.GetByExaminationAsync(request.ExaminationId);
        foreach(Question q in questions){
            var studentStats = await _statsRepository.GetAllStudentStatsInQuestion(q.Id);
            foreach(var s in studentStats){
                await _statsRepository.DeleteStudentStats(s);
            }
            q.UpdateModified(true);
            await _questionRepository.UpdateQuestion(q);
        }

        await _examinationRepository.UpdateAsync(examination);
        return examination;
    }
}
