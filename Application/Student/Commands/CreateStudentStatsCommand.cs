using Application.Questions.Commands;
using Domain.Entities;
using MediatR;

namespace Application.Student.Commands;

public record CreateStudentStatsCommand(
    string UserId,
    string QuestionId,
    List<ProblemCommand> Problems,
    List<ExaminationSelectionCommand> Examinations,
    List<DiagnosticCommand> Diagnostics,
    List<TreatmentCommand> Treatments,
    int HeartProblem1,
    int HeartProblem2,
    string? ExtraAns
) : IRequest<StudentStats>;

public record ExaminationSelectionCommand(
    string Id,
    int Round
);