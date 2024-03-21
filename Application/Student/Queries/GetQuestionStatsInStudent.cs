using Domain.Entities;
using MediatR;

namespace Application.Student.Queries;

public record GetQuestionStatsInStudent(
    string QuestionId,
    string UserId
) : IRequest<QuestionStatsResult>;