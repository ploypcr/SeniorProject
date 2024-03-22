using Application.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TreatmentRepository : ITreatmentRepository
{
    private readonly HistoryTakingDb _context;
    public TreatmentRepository(HistoryTakingDb context)
    {
        _context = context;
    }

    public async Task AddAsync(Treatment t)
    {
        await _context.Treatments.AddAsync(t);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Treatment treatment)
    {
        _context.Treatments.Remove(treatment);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Treatment>> GetAllTreatmentAsync()
    {
        return await _context.Treatments.ToListAsync();
    }

    public async Task<Treatment?> GetByIdAsync(TreatmentId id)
    {
        return await _context.Treatments.FirstOrDefaultAsync(treatment => treatment.Id == id);

    }

    public async Task<Treatment?> GetByNameAndTypeAsync(string name, string type)
    {
        return await _context.Treatments.FirstOrDefaultAsync(treatment => treatment.Name == name && treatment.Type == type);
    }

    public async Task UpdateAsync(Treatment treatment)
    {
        _context.Treatments.Update(treatment);
        await _context.SaveChangesAsync();
    }
}