using Domain.Entities;

namespace Application.Abstractions;

public interface ITagRepository{
    Task AddAsync(Tag t);
    Task<Tag?> GetByNameAsync(string name);
    Task<List<Tag>> GetAllTagsAsync();
    Task<Tag?> GetByIdAsync(TagId id);
    Task DeleteAsync(Tag tag);
    Task UpdateAsync(Tag tag);
};