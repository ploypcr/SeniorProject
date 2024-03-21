using Domain.Entities;
using MediatR;

namespace Application.Examinations.Queries;

public record GetExaminationById(
    string Id
) : IRequest<Examination>;