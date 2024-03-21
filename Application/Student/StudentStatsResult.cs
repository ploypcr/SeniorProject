using Domain.Entities;

public record StudentStatsResult(
    User Student,
    StudentStats StudentStats,
    List<StudentExaminationsResult> Examination,
    List<StudentProblemsResult> Problem
);

public record StudentProblemsResult(
    string Id,
    string Name,
    int Round
);

public record StudentExaminationsResult(
    string Id,
    string Name,
    string Type,
    string Lab,
    string Area,
    int Cost
);