
using Domain.Entities;
using MediatR;

public record GetExaminationResultCommand(
    string QuestionId,
    string ExaminationId
) : IRequest<QuestionExamination>;