using Application.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProblemRepository : IProblemRepository
{
    private readonly HistoryTakingDb _context;
    public ProblemRepository(HistoryTakingDb context)
    {
        _context = context;
    }

    public async Task AddAsync(Problem p)
    {
        await _context.Problems.AddAsync(p);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Problem problem)
    {
        _context.Problems.Remove(problem);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Problem>> GetAllProblemsAsync()
    {
        return await _context.Problems.ToListAsync();
    }

    public async Task<Problem?> GetByIdAsync(ProblemId problemId)
    {
        return await _context.Problems.FirstOrDefaultAsync(problem => problem.Id == problemId);
    }

    public async Task<Problem?> GetByNameAsync(string name)
    {
        return await _context.Problems.FirstOrDefaultAsync(problem => problem.Name == name);
    }

    public async Task UpdateAsync(Problem problem)
    {
        _context.Problems.Update(problem);
        await _context.SaveChangesAsync();
    }
}