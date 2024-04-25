using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Questions.Commands;

public record CreateQuestionCommand(
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

public record ProblemCommand(
    string Id,
    int Round
);

public record SignalmentCommand(
    string Species,
    string Breed,
    string Gender,
    bool Sterilize,
    string Age,
    string Weight
);

public record ExaminationCommand(
    string Id,
    string TextResult,
    string ImgResult
);

public record TreatmentCommand(
    string Id
);

public record DiagnosticCommand(
    string Id
);

public record TagCommand(
    string Id
);