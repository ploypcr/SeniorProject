using Domain.Entities;
using MediatR;

namespace Application.Examinations.Commands;
public record CreateExaminationCommand(
    string Id,
    string Lab,
    string Name,
    string? Type,
    string? Area,
    string? TextDefault,
    string? ImgDefault,
    int Cost
): IRequest<Examination>;