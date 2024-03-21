using Domain.Entities;

public record QuestionStatsResult(
    Question Question,
    StudentStats StudentStats,
    List<StudentExaminationsResult> Examination,
    List<StudentProblemsResult> Problem
);