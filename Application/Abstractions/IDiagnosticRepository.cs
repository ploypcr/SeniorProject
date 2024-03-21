using Domain.Entities;

namespace Application.Abstractions;

public interface IDiagnosticRepository{
    Task AddAsync(Diagnostic d);
    Task<Diagnostic?> GetByNameAndTypeAsync(string name, string type);
    Task<List<Diagnostic>> GetAllDiagnosticsAsync();
    Task<Diagnostic?> GetByIdAsync(DiagnosticId id);
    Task DeleteAsync(Diagnostic diagnostic);
    Task UpdateAsync(Diagnostic diagnostic);
};