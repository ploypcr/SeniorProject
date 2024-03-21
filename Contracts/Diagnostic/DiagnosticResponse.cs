namespace Contracts.Diagnostic;
public record DiagnosticResponse(
    Guid Id,
    string Name,
    string Type
);