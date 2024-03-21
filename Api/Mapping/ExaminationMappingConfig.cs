using Application.Examinations.Commands;
using Application.Questions.Commands;
using Application.Student.Commands;
using Contracts.Examination;
using Contracts.Question;
using Contracts.Student;
using Domain.Entities;
using Mapster;

namespace Api.Mapping;

public class ExaminationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ExaminationRequest, CreateExaminationCommand>()
            .Map(dest => dest, src => src);
        config.NewConfig<Examination, ExaminationResponse>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString())
            .Map(dest => dest.ImgPath, src => src.ImgDefault)
            .Map(dest => dest, src => src);

        config.NewConfig<ExaminationResult, ExaminationResponse>()
            .Map(dest => dest.Id, src => src.Id.ToString())
            .Map(dest => dest.TextDefault, src => src.TextResult)
            .Map(dest => dest.ImgPath, src => src.ImgResult)
            .Map(dest => dest, src => src);

        config.NewConfig<ExaminationResult, QuestionExaminationResponse>()
            .Map(dest => dest.Id, src => src.Id.ToString())
            .Map(dest => dest.ImgPath, src => src.ImgResult)
            .Map(dest => dest, src => src);

        config.NewConfig<ExaminationSelection,ExaminationSelectionCommand>()
            .Map(dest => dest, src => src);
        
        config.NewConfig<StudentExamination, StudentExaminationResponse>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString())
            .Map(dest => dest, src => src);

        config.NewConfig<(GetExaminationRequest Request, string QuestionId), GetExaminationResultCommand>()
            .Map(dest => dest, src => src.Request)
            .Map(dest => dest.QuestionId, src => src.QuestionId);
        
        config.NewConfig<QuestionExamination, QuestionExaminationResult>()
            .Map(dest => dest.Id, src => src.ExaminationId.Value.ToString())
            .Map(dest => dest, src => src);

        config.NewConfig<ExaminationRequest, UpdateExaminationCommand>()
            .Map(dest => dest.ExaminationId, src => new ExaminationId(new Guid(src.Id)))
            .Map(dest => dest, src => src);

        
        config.NewConfig<DeleteExaminationRequest, DeleteExaminationCommand>()
            .Map(dest => dest.ExaminationId, src => new ExaminationId(new Guid(src.Id)));
    }
}