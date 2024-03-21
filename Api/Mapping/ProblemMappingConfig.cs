using Application.Problems.Commands;
using Contracts.Problem;
using Contracts.Question;
using Contracts.Student;
using Domain.Entities;
using Mapster;

namespace Api.Mapping;

public class ProblemMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Problem, ProblemResponse>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString())
            .Map(dest => dest, src => src);

        config.NewConfig<ProblemResult, QuestionProblemResponse>()
            .Map(dest => dest.Id, src => src.Id.ToString())
            .Map(dest => dest, src => src);

        config.NewConfig<StudentProblem, StudentProblemResponse>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString())
            .Map(dest => dest, src => src);
        
        config.NewConfig<CreateProblemRequest, CreateProblemCommand>()
            .Map(dest => dest, src => src);

        config.NewConfig<UpdateProblemRequest, UpdateProblemCommand>()
            .Map(dest => dest.ProblemId, src => new ProblemId(new Guid(src.Id)))
            .Map(dest => dest, src => src);

        config.NewConfig<DeleteProblemRequest, DeleteProblemCommand>()
            .Map(dest => dest.ProblemId, src => new ProblemId(new Guid(src.Id)));
    }
}