namespace Contracts.Examination;

public record ExaminationResponse(
    string Id,
    string Lab,
    string Name,
    string? Type,
    string? Area,
    int Cost,
    string TextDefault,
    string ImgPath
);