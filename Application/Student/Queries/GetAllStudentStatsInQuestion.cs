using Domain.Entities;
using MediatR;

namespace Application.Student.Queries;

public record GetAllStudentStatsInQuestion(
    string QuestionId
) : IRequest<List<StudentStatsResult>>;