using Domain.Entities;
using MediatR;
namespace  Application.Questions.Commands;
public record UpdateQuestionCommand(
    string Id,
    string Name,
    string ClientComplains,
    string HistoryTakingInfo,
    string GeneralInfo,
    SignalmentCommand Signalment,
    List<ProblemCommand> Problems,
    List<ExaminationCommand> Examinations,
    List<TreatmentCommand> Treatments,
    List<DiagnosticCommand> Diagnostics,
    List<TagCommand> Tags,
    string UserId,
    int Status,
    string ExtraQues
) : IRequest<Question>;