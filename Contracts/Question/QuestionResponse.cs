using Contracts.Tag;

namespace Contracts.Question;

public record QuestionResponse(
    Guid Id ,
    string QuesVersion,
    string? Name,
    string? ClientComplains,
    string?  HistoryTakingInfo,
    string?  GeneralInfo,
    List<TagResponse> Tags,
    SignalmentResponse Signalment
);