using Domain.Entities;
using MediatR;

namespace Application.Examinations.Queries;

public record GetAllExaminations(

) : IRequest<List<Examination>>;