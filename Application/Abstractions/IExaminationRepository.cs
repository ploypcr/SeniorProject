using Domain.Entities;

namespace Application.Abstractions;

public interface IExaminationRepository{
    Task<Examination?> GetByIdAsync(ExaminationId id);
    Task<List<Examination>> GetAllExaminationsAsync();
    Task<Examination?> GetByDetails(string name, string type, string lab, string area);
    Task<Examination?> AddAsync(Examination e);

    Task DeleteAsync(Examination e);
    Task UpdateAsync(Examination e);
};