using Microsoft.AspNetCore.Http;

namespace Contracts.Question;


public record CreateOrUpdateQuestionRequest(
    string? Id,
    string? ClientComplains,
    string? HistoryTakingInfo,
    string? GeneralInfo,
    SignalmentRequest? Signalment,
    List<QuestionProblemRequest>? Problems,
    List<QuestionExaminationRequest>? Examinations,
    List<QuestionTreatmentRequest>? Treatments,
    List<QuestionDiagnosticRequest>? Diagnostics,
    List<QuestionTagRequest>? Tags,
    int Status,
    string? ExtraQues
);

public record QuestionProblemRequest(
    string Id,
    int Round
);

public record SignalmentRequest(
    string? Species,
    string? Breed,
    string? Gender,

    bool? Sterilize,
    string? Age,
    string? Weight
);

public record QuestionExaminationRequest(
    string Id,
    string? TextResult,
    IFormFile? ImgResult,
    string? ImgPath
);

public record QuestionTreatmentRequest(
    string Id
);

public record QuestionTagRequest(
    string Id
);

public record QuestionDiagnosticRequest(
    string Id
);