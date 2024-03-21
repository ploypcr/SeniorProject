using Application.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class StatsRepository : IStatsRepository
{
    private readonly HistoryTakingDb _context;
    public StatsRepository(HistoryTakingDb context){
        _context = context;
    }
    public async Task AddStudentStats(StudentStats studentStats)
    {
        await _context.StudentStats.AddAsync(studentStats);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteStudentStats(StudentStats studentStats)
    {
        _context.StudentStats.Remove(studentStats);
        await _context.SaveChangesAsync();
    }
    public async Task<List<StudentStats>> GetAllQuestionStatsInStudent(string userId)
    {
        return await _context.StudentStats
                        .Where(s => s.UserId == userId)
                        .Include(s => s.Examinations)
                        .Include(s => s.Problems)
                        .Include(s => s.Treatments)
                        .Include(s => s.Diagnostics)
                        .AsSplitQuery()
                        .ToListAsync();
    }

    public async Task<List<StudentStats>> GetAllStudentStatsInQuestion(QuestionId questionId)
    {
        return await _context.StudentStats
                        .Where(s => s.QuestionId == questionId)
                        .Include(s => s.Examinations)
                        .Include(s => s.Problems)
                        .Include(s => s.Treatments)
                        .Include(s => s.Diagnostics)
                        .AsSplitQuery()
                        .ToListAsync();

    }

    public async Task<StudentStats> GetStudentStats(string userId, QuestionId questionId)
    {
        return await _context.StudentStats
                        .Where(s => s.QuestionId ==questionId && s.UserId == userId)
                        .Include(s => s.Examinations)
                        .Include(s => s.Problems)
                        .Include(s => s.Treatments)
                        .Include(s => s.Diagnostics)
                        .OrderByDescending(s => s.DateTime)
                        .AsSplitQuery()
                        .FirstOrDefaultAsync();
    }
}