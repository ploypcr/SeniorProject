namespace Contracts.Treatment;
public record CreateTreatmentRequest(
    string Name,
    string Type,
    int Cost
);