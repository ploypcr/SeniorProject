using Application.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DiagnosticRepository : IDiagnosticRepository
{
    private readonly HistoryTakingDb _context;
    public DiagnosticRepository(HistoryTakingDb context){
        _context = context;
    }
    public async Task AddAsync(Diagnostic d)
    {
        await _context.Diagnostics.AddAsync(d);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Diagnostic diagnostic)
    {
        _context.Diagnostics.Remove(diagnostic);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Diagnostic>> GetAllDiagnosticsAsync()
    {
        return await _context.Diagnostics.ToListAsync();
    }

    public async Task<Diagnostic?> GetByIdAsync(DiagnosticId id)
    {
        return await _context.Diagnostics.FirstOrDefaultAsync(diagnostic => diagnostic.Id == id);
    }

    public async Task<Diagnostic?> GetByNameAndTypeAsync(string name, string type)
    {
        return await _context.Diagnostics.FirstOrDefaultAsync(diagnostic => diagnostic.Name == name && diagnostic.Type == type);
    }

    public async Task UpdateAsync(Diagnostic diagnostic)
    {
        _context.Diagnostics.Update(diagnostic);
        await _context.SaveChangesAsync();
    }
}