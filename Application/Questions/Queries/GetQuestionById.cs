using Domain.Entities;
using MediatR;

namespace Application.Questions.Queries;
public record GetQuestionById(
    QuestionId QuestionId
) : IRequest<QuestionResult>;