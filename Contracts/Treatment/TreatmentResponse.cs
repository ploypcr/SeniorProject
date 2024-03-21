namespace Contracts.Treatment;

public record TreatmentResponse(
    Guid Id,
    string Type,
    string Name,
    int Cost
);