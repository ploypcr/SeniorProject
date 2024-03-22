using Domain.Entities;

namespace Application.Abstractions;

public interface ITreatmentRepository{
    Task AddAsync(Treatment t);
    Task<Treatment?> GetByNameAndTypeAsync(string name, string type);
    Task<List<Treatment>> GetAllTreatmentAsync();
    Task<Treatment?> GetByIdAsync(TreatmentId id);
    Task DeleteAsync(Treatment treatment);
    Task UpdateAsync(Treatment treatment);
};