using Domain.Entities;
using MediatR;

namespace Application.Student.Queries;

public record GetAllQuestionStatsInStudent(
    string UserId
) : IRequest<List<QuestionStatsResult>>;