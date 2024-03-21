namespace Contracts.Treatment;
public record UpdateTreatmentRequest(
    string Id,
    string Name,
    string? Type,
    int Cost
);