using Contracts.Diagnostic;
using Contracts.Examination;
using Contracts.Problem;
using Contracts.Tag;
using Contracts.Treatment;

namespace Contracts.Question;

public record QuestionWithSolutionResponse(
    Guid Id ,
    string QuesVersion,
    string? Name,
    string? ClientComplains,
    string?  HistoryTakingInfo,
    string?  GeneralInfo,
    List<QuestionProblemResponse> Problems,
    List<TreatmentResponse> Treatments,
    List<DiagnosticResponse> Diagnostics,
    List<QuestionExaminationResponse> Examinations,
    List<TagResponse> Tags,
    List<LogResponse> Logs,
    SignalmentResponse Signalment,
    int Modified,
    int Status,
    string? ExtraQues
);

public record QuestionProblemResponse(
    string Id,
    string Name,
    int Round
);
public record QuestionExaminationResponse(
    string Id,
    string Lab,
    string Name,
    string? Type,
    string? Area,
    int Cost,
    string TextResult,
    string ImgPath
);
public record SignalmentResponse(
    string Breed,
    string Species,
    string Gender,
    bool Sterilize,
    string Age,
    string Weight
);

public record LogResponse(
    string Name,
    DateTime DateTime
);