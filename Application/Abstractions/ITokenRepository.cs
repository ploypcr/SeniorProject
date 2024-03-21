using Domain.Entities;

namespace Application.Abstractions;

public interface ITokenRepository{
    Task<RefreshToken?> GetAllUserToken(string email);
    Task<User?> GetUserByIdAsync(string userId);
    //Task<User?> GetUserByUserNameAsync(string userName);

    // Task<bool> CheckPasswordAsync(User user, string password);

}