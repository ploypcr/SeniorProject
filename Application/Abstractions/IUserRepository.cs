using Domain.Entities;

namespace Application.Abstractions;

public interface IUserRepository{
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdAsync(string id);

    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);

}