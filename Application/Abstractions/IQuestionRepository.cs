using Domain.Entities;

namespace Application.Abstractions;

public interface IQuestionRepository{
    Task<Question> AddQuestion(Question q);
    Task<Question?> GetByIdAsync(QuestionId questionId);
    Task<Question?> GetByNameAsync(int name);
    Task<int> GetLastestName();



    Task<List<Question>> GetByExaminationAsync(ExaminationId examinationId);
    Task<List<Question>> GetByTreatmentAsync(TreatmentId treatmentId);
    Task<List<Question>> GetByTagAsync(TagId tagId);
    Task<List<Question>> GetByProblemAsync(ProblemId problemId);
    Task<List<Question>> GetByDiagnosticAsync(DiagnosticId diagnosticId);

    Task<QuestionExamination?> GetExaminationResult(QuestionId questionId, ExaminationId examinationId);
    Task UpdateQuestion(Question q);
    Task<List<Question>> GetAllQuestionsAsync (List<string>? tagSearch);
    Task DeleteQuestionAsync(Question question);
};