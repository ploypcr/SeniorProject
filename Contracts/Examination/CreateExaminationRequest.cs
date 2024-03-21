using Microsoft.AspNetCore.Http;

namespace Contracts.Examination;

public record CreateExaminationRequest(
    List<ExaminationRequest> Examinations
);

public record ExaminationRequest(
    string? Id,
    string? Lab,
    string? Name,
    string? Type,
    string? Area,
    int Cost,
    string? TextDefault,
    IFormFile? ImgDefault,
    string? ImgPath
);