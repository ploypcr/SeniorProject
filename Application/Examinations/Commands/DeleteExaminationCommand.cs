using Domain.Entities;
using MediatR;

namespace Application.Examinations.Commands;

public record DeleteExaminationCommand(
    ExaminationId ExaminationId
) : IRequest;