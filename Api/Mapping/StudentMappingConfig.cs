using Application.Student.Commands;
using Contracts.Student;
using Domain.Entities;
using Mapster;

namespace Api.Mapping;
public class StudentMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(CreateStudentStatsRequest request, string userId, string questionId),CreateStudentStatsCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest.QuestionId, src=> src.questionId)
            .Map(dest => dest, src => src.request);

        config.NewConfig<StudentStatsResult, StudentStatsResponse>()
            .Map(dest => dest.UserId, src => src.Student.Id)
            .Map(dest => dest.UserName, src => src.Student.FirstName + " " + src.Student.LastName)
            .Map(dest => dest.Diagnostics, src => src.StudentStats.Diagnostics)
            .Map(dest => dest.Treatments, src => src.StudentStats.Treatments)
            .Map(dest => dest.Problems, src => src.Problem)
            .Map(dest => dest.Examinations, src => src.Examination)
            .Map(dest => dest.Problem1_Score, src => 100)
            .Map(dest => dest.Problem2_Score, src => src.StudentStats.Problem2_Score)
            .Map(dest => dest.Examination_Score, src => src.StudentStats.Examination_Score)
            .Map(dest => dest.Treatment_Score, src => src.StudentStats.Treatment_Score)
            .Map(dest => dest.DiffDiag_Score, src => src.StudentStats.Diff_Diagnostic_Score)
            .Map(dest => dest.TenDiag_Score, src => src.StudentStats.Ten_Diagnostic_Score);



        config.NewConfig<QuestionStatsResult, QuestionStatsResponse>()
            .Map(dest => dest.QuestionId, src => src.Question.Id.Value.ToString())
            .Map(dest => dest.QuestionName, src => src.Question.Name)
            .Map(dest => dest.Diagnostics, src => src.StudentStats.Diagnostics)
            .Map(dest => dest.Treatments, src => src.StudentStats.Treatments)
            .Map(dest => dest.Problems, src => src.Problem)
            .Map(dest => dest.Examinations, src => src.Examination)
            .Map(dest => dest.Problem1_Score, src => src.StudentStats.Problem1_Score)
            .Map(dest => dest.Problem2_Score, src => src.StudentStats.Problem2_Score)
            .Map(dest => dest.Examination_Score, src => src.StudentStats.Examination_Score)
            .Map(dest => dest.Treatment_Score, src => src.StudentStats.Treatment_Score)
            .Map(dest => dest.DiffDiag_Score, src => src.StudentStats.Diff_Diagnostic_Score)
            .Map(dest => dest.TenDiag_Score, src => src.StudentStats.Ten_Diagnostic_Score);


        config.NewConfig<StudentProblemsResult, StudentProblemResponse>()
            .Map(dest => dest, src => src);

        config.NewConfig<StudentExaminationsResult, StudentExaminationResponse>()
            .Map(dest => dest, src => src);
    }
}