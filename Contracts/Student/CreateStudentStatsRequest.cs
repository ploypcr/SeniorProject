using Contracts.Question;

namespace Contracts.Student;

public record CreateStudentStatsRequest(
    List<QuestionProblemRequest> Problems,
    List<ExaminationSelection> Examinations,
    List<QuestionTreatmentRequest> Treatments,
    List<QuestionDiagnosticRequest> Diagnostics,
    int HeartProblem1,
    int HeartProblem2,
    string? ExtraAns
);

public record ExaminationSelection(
    string Id
);
