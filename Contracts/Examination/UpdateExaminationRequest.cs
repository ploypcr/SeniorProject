using Microsoft.AspNetCore.Http;

namespace Contracts.Examination;

public record UpdateExaminationRequest(
    List<ExaminationRequest> Examinations
);