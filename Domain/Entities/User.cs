namespace Domain.Entities;

public class User{
    public string Id {get; private set;}
    public string FirstName {get; private set;}
    public string LastName {get; private set;}
    public string StudentId {get; private set;}

    public string Email {get; private set;}
    public string? Password {get; private set;}
    private readonly List<RefreshToken> _refreshTokens = new();
    public IReadOnlyList<RefreshToken> RefreshTokens => _refreshTokens.ToList();


    public static User Create(
        string firstName,
        string lastName,
        string studentId,
        string email,
        string password
    ){
        return new User{
            StudentId = studentId,
            Id = Guid.NewGuid().ToString(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };
    }

    public void AddRefreshToken(
        RefreshToken refreshToken
    ){
        _refreshTokens.Add(refreshToken);
    }
    public void RemoveRefreshToken(
        RefreshToken refreshToken
    ){
        _refreshTokens.Remove(refreshToken);
    }
}