using System;
namespace Domain.Entities;

public class Examination{
    public ExaminationId Id { get; private set; }
    public string? Name { get; private set; }
    public string? Lab { get; private set; }
    public string? Area { get; private set; }
    public string? Type { get; private set; }
    public string? TextDefault { get; private set; }
    public string? ImgDefault { get; private set; }
    public int Cost { get; private set; }
    public static Examination Create(
        string id,
        string lab,
        string name,
        string? type,
        string? area,
        int cost,
        string textDefault,
        string imgDefault
    ){
        var examination = new Examination{
            Id = new ExaminationId(Guid.NewGuid()),
            Lab = lab,
            Name = name,
            Type = type,
            Cost = cost,
            Area = area,
            TextDefault = textDefault,
            ImgDefault = imgDefault
        };

        return examination;
    }
    public void Update(
        string lab,
        string name,
        string? type,
        string? area,
        int cost,
        string textDefault,
        string imgDefault
    ){
        Lab = lab;
        Name = name;
        Type = type;
        Area = area;
        Cost = cost;
        TextDefault = textDefault;
        ImgDefault = imgDefault;
    }

};