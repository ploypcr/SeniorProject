using Domain.Entities;

namespace Application.Abstractions;
public interface IStatsRepository{
    Task<List<StudentStats>> GetAllStudentStatsInQuestion(QuestionId questionId);
    Task<List<StudentStats>> GetAllQuestionStatsInStudent(string userId);

    Task<StudentStats> GetStudentStats(string userId, QuestionId questionId);
    Task AddStudentStats(StudentStats studentSelection);
    Task DeleteStudentStats(StudentStats studentSelection);


}