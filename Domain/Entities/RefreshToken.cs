namespace Domain.Entities;
public class RefreshToken{
    public string Id {get; private set;}
    public string Token {get; private set;}
    public DateTime CreatedTime {get; private set;}
    public string UserId {get; private set;}

    public static RefreshToken Create(string token, string uid){
        return new RefreshToken{
            Id = Guid.NewGuid().ToString(),
            Token = token,
            CreatedTime = DateTime.UtcNow,
            UserId = uid
        };
    }

    public void Update(string token){
        Token = token;
        CreatedTime = DateTime.UtcNow;
    }
}