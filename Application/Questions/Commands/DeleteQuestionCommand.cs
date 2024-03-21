using Domain.Entities;
using MediatR;

namespace Application.Questions.Commands;
public record DeleteQuestionCommand(
    QuestionId QuestionId
) : IRequest;