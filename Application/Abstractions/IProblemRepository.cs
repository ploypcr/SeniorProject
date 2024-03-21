using Domain.Entities;

namespace Application.Abstractions;

public interface IProblemRepository{
    Task AddAsync(Problem p);
    Task<Problem?> GetByNameAsync(string name);
    Task<List<Problem>> GetAllProblemsAsync();
    Task<Problem?> GetByIdAsync(ProblemId problemId);
    Task DeleteAsync(Problem problem);
    Task UpdateAsync(Problem problem);

};