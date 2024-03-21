using Domain.Entities;
using MediatR;

namespace Application.Questions.Queries;

public record GetAllQuestions(
    List<string>? TagSearch
) : IRequest<List<QuestionResult>>;