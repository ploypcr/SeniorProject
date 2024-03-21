namespace Domain.Entities;
public class Treatment{
    public TreatmentId Id { get; private set; }
    public string Type { get; private set; }
    public string Name { get; private set; }
    public int Cost { get; private set; }


    public static Treatment Create(
        string type,
        string name,
        int cost
    ){
        return new Treatment{
            Id = new TreatmentId(Guid.NewGuid()),
            Type = type,
            Name = name,
            Cost = cost,
        };
    }

    public void Update(
        string type,
        string name,
        int cost
    ){
        Type = type;
        Name = name;
        Cost = cost;
    }
}