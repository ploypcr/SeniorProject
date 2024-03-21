using Domain.Entities;
using MediatR;

namespace Application.Treatments.Queries;

public record GetAllTreatments(

): IRequest<List<Treatment>>;