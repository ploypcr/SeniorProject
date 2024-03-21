using Application.Diagnostics.Commands;
using Contracts.Diagnostic;
using Domain.Entities;
using Mapster;

namespace Api.Mapping;

public class DiagnosticMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Diagnostic, DiagnosticResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest, src => src);
        config.NewConfig<CreateDiagnosticRequest, CreateDiagnosticCommand>()
            .Map(dest => dest, src => src);

        config.NewConfig<UpdateDiagnosticRequest, UpdateDiagnosticCommand>()
            .Map(dest => dest.DiagnosticId, src => new DiagnosticId(new Guid(src.Id)))
            .Map(dest => dest, src => src);

        config.NewConfig<DeleteDiagnosticRequest, DeleteDiagnosticCommand>()
            .Map(dest => dest.DiagnosticId, src => new DiagnosticId(new Guid(src.Id)));
    }
}
