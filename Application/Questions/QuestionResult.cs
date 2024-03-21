using Domain.Entities;

public record QuestionResult(
    Question Question,
    List<ProblemResult> Problems,
    List<ExaminationResult> Examinations,
    List<LogResult> Logs
);

public record ProblemResult(
    Guid Id,
    string Name,
    int Round
);

public record ExaminationResult(
    Guid Id,
    string Lab,
    string Type,
    string Name,
    string Area,
    int Cost,
    string? TextResult,
    string? ImgResult
);

public record LogResult(
    string Name,
    DateTime DateTime
);