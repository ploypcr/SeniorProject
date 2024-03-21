namespace Domain.Entities;

public class Tag{
    public TagId Id {get; private set;}
    public string Name {get; private set;}

    public static Tag Create(string name){
        return new Tag{
            Id = new TagId(Guid.NewGuid()),
            Name = name
        };
    }
    public void Update(string name){
        Name = name;
    }
}