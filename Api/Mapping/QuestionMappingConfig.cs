using Application.Questions.Commands;
using Contracts.Diagnostic;
using Contracts.Examination;
using Contracts.Problem;
using Contracts.Question;
using Contracts.Tag;
using Contracts.Treatment;
using Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.DataProtection;
namespace Api.Mapping;

public class QuestionMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(CreateOrUpdateQuestionRequest Request, string UserId, List<ExaminationCommand> Examinations), CreateQuestionCommand>()
            .Map(dest => dest, src => src.Request)
            .Map(dest => dest.Examinations, src => src.Examinations)
            .Map(dest => dest.UserId, src => src.UserId);

        config.NewConfig<(CreateOrUpdateQuestionRequest Request, string UserId, string QuestionId, List<ExaminationCommand> Examinations), UpdateQuestionCommand>()
            .Map(dest => dest, src => src.Request)
            .Map(dest => dest.Examinations, src => src.Examinations)
            .Map(dest => dest.Id, src => src.QuestionId)
            .Map(dest => dest.UserId, src => src.UserId);

        config.NewConfig<QuestionResult, QuestionWithSolutionResponse>()
            .Map(dest => dest.Id, src => src.Question.Id.Value)
            .Map(dest => dest.QuesVersion, src => String.Format("{0:0.0}",src.Question.QuesVersion))
            .Map(dest => dest.Name, src => src.Question.Name)
            .Map(dest => dest.ClientComplains, src => src.Question.ClientComplains)
            .Map(dest => dest.HistoryTakingInfo, src => src.Question.HistoryTakingInfo)
            .Map(dest => dest.GeneralInfo, src => src.Question.GeneralInfo)
            .Map(dest => dest.Diagnostics, src => src.Question.Diagnostics)
            .Map(dest => dest.Treatments, src => src.Question.Treatments)
            .Map(dest => dest.Signalment, src => src.Question.Signalment)
            .Map(dest => dest.Tags, src => src.Question.Tags)
            .Map(dest => dest.Modified, src => src.Question.Modified)
            .Map(dest => dest.ExtraQues, src => src.Question.ExtraQues)
            .Map(dest => dest.Status, src => src.Question.Status);
            
            
        config.NewConfig<QuestionResult, QuestionResponse>()
            .Map(dest => dest.Id, src => src.Question.Id.Value)
            .Map(dest => dest.QuesVersion, src => String.Format("{0:0.0}",src.Question.QuesVersion))
            .Map(dest => dest.Name, src => src.Question.Name)
            .Map(dest => dest.HistoryTakingInfo, src => src.Question.HistoryTakingInfo)
            .Map(dest => dest.GeneralInfo, src => src.Question.GeneralInfo)
            .Map(dest => dest.ClientComplains, src => src.Question.ClientComplains)
            .Map(dest => dest.Signalment, src => src.Question.Signalment)
            .Map(dest => dest.ExtraQues, src => src.Question.ExtraQues)
            .Map(dest => dest.Tags, src => src.Question.Tags);
        config.NewConfig<QuestionProblemRequest, ProblemCommand>()
            .Map(dest => dest, src => src);
        config.NewConfig<Tag, TagResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name);

        config.NewConfig<LogResult, LogResponse>()
            .Map(dest => dest, src => src);

        config.NewConfig<Signalment, SignalmentResponse>()
            .Map(dest => dest, src => src);

        config.NewConfig<Treatment, TreatmentResponse>()
            .Map(dest => dest, src => src)
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<Diagnostic, DiagnosticResponse>()
            .Map(dest => dest, src => src)
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<Treatment, TreatmentResponse>()
            .Map(dest => dest, src => src)
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<ProblemResult, ProblemResponse>()
            .Map(dest => dest, src => src);
        
            
    }
}