using Application.Questions.Commands;
using Application.Student.Commands;
using Application.Treatments.Commands;
using Contracts.Examination;
using Contracts.Student;
using Contracts.Treatment;
using Domain.Entities;
using Mapster;

namespace Api.Mapping;

public class TreatmentMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateTreatmentRequest, CreateTreatmentCommand>()
            .Map(dest => dest, src => src);

        config.NewConfig<Treatment, TreatmentResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest, src => src);
        
        config.NewConfig<UpdateTreatmentRequest, UpdateTreatmentCommand>()
            .Map(dest => dest.TreatmentId, src => new TreatmentId(new Guid(src.Id)))
            .Map(dest => dest, src => src);

        config.NewConfig<DeleteTreatmentRequest, DeleteTreatmentCommand>()
            .Map(dest => dest.TreatmentId, src => new TreatmentId(new Guid(src.Id)));

    }
}