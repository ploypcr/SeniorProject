namespace Domain.Entities;
public class Diagnostic{
    public DiagnosticId Id { get; private set; }
    public string Name { get; private set; }
    public string Type { get; private set; }


    public static Diagnostic Create(string name, string type){
        return new Diagnostic{
            Id = new DiagnosticId(Guid.NewGuid()),
            Name = name,
            Type = type
        };
    }

    public void  Update(string name){
        Name = name;
    }
};
