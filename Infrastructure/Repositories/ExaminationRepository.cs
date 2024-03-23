using Application.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ExaminationRepository : IExaminationRepository
{
    private readonly HistoryTakingDb _context;
    public ExaminationRepository(HistoryTakingDb context){
        _context = context;
    }

    public async Task<Examination?> AddAsync(Examination e)
    {
        _context.Examinations.Add(e);
        await _context.SaveChangesAsync();
        return e;
    }

    public async Task DeleteAsync(Examination e)
    {
        _context.Examinations.Remove(e);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Examination>> GetAllExaminationsAsync()
    {
        return await _context.Examinations.ToListAsync();
    }

    public async Task<Examination?> GetByIdAsync(ExaminationId id)
    {
        return await _context.Examinations.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Examination?> GetByDetails(string name, string type, string lab, string area)
    {
        return await _context.Examinations.FirstOrDefaultAsync(e => e.Name == name && e.Type == type && e.Lab == lab && e.Area == area);
    }

    public async Task UpdateAsync(Examination e)
    {
        _context.Examinations.Update(e);
        await _context.SaveChangesAsync();
    }
}