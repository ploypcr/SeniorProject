using System;
using Contracts.Diagnostic;
using Contracts.Treatment;

namespace Contracts.Student;

public record StudentStatsResponse(
    string UserId,
    string UserName,
    List<StudentExaminationResponse> Examinations,
    List<StudentProblemResponse> Problems,
    List<TreatmentResponse> Treatments,
    List<DiagnosticResponse> Diagnostics,
    double Problem1_Score,
    double Problem2_Score,
    double Examination_Score,
    double Treatment_Score,
    double DiffDiag_Score,
    double TenDiag_Score,
    DateTime DateTime
);

public record StudentExaminationResponse(
    string Id,
    string Name,
    string Type,
    string Lab,
    string Area,
    int Cost
);

public record StudentProblemResponse(
    string Id,
    string Name,
    int Round
);