namespace Domain.Entities;

public class Problem{
    public ProblemId Id { get; private set;}
    public string Name { get; private set; }

    public static Problem Create(
        string name
    ){
        var problem = new Problem{
            Id = new ProblemId(Guid.NewGuid()),
            Name = name
        };

        return problem;
    }

    public void  Update(string name){
        Name = name;
    }
};