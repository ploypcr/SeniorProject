using Domain.Entities;
using MediatR;

namespace Application.Examinations.Commands;

public record UpdateExaminationCommand(
    ExaminationId ExaminationId,
    string? Lab,
    string? Name,
    string? Type,
    string? Area,
    string? TextDefault,
    int Cost,
    string? ImgDefault
) : IRequest<Examination>;