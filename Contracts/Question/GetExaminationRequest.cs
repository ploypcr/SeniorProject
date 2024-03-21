
namespace Contracts.Question;
public record GetExaminationRequest(
    string ExaminationId
);
public record GetExaminationResultRequest(
    List<GetExaminationRequest> Examinations
);
