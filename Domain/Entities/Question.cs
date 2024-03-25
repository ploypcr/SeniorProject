namespace Domain.Entities;

public class Question{
    private readonly List<Diagnostic> _diagnostics = new();
    public IReadOnlyList<Diagnostic> Diagnostics => _diagnostics.ToList();

    public QuestionId Id { get; private set; }
    public int Name { get; private set; }
    public string? ClientComplains { get; private set; } 
    public string?  HistoryTakingInfo { get; private set; }
    public string?  GeneralInfo { get; private set; }
    private readonly List<QuestionProblem> _problems = new();
    private readonly List<Treatment> _treatments = new();
    private readonly List<QuestionExamination> _examinations = new();
    private readonly List<Tag> _tags = new();
    private readonly List<QuestionLog> _logs = new();

    public IReadOnlyList<QuestionProblem> Problems => _problems.ToList();
    public IReadOnlyList<Treatment> Treatments => _treatments.ToList();
    public IReadOnlyList<QuestionExamination> Examinations => _examinations.ToList();
    public IReadOnlyList<Tag> Tags => _tags.ToList();

    public IReadOnlyList<QuestionLog> Logs => _logs.ToList();
    public Signalment? Signalment { get; private set; }
    public bool Modified { get; private set; }
    public int Status { get; private set; }


    public static Question Create(
        int name,
        string clientComplains,
        string historyTakingInfo,
        string generalInfo,
        Signalment signalment,
        int status
    ){
        var question = new Question{
            Id = new QuestionId(Guid.NewGuid()),
            Name = name,
            ClientComplains = clientComplains,
            HistoryTakingInfo = historyTakingInfo,
            GeneralInfo = generalInfo,
            Signalment = signalment,
            Status = status
        };

        return question;
    }

    public void UpdateQuestion(int name, string clientComplains, string historyTakingInfo, string generalInfo, Signalment signalment, int status){
        Name = name;
        ClientComplains = clientComplains;
        HistoryTakingInfo = historyTakingInfo;
        GeneralInfo = generalInfo;
        Signalment = signalment;
        Status = status;

    }
    public void AddProblem(ProblemId problemId, int round){
        var questionproblem = new QuestionProblem(
            new QuestionProblemId(Guid.NewGuid()),
            problemId,
            Id,
            round
        );

        _problems.Add(questionproblem);
    }

    public void RemoveProblem(QuestionProblem questionProblem){;
        _problems.Remove(questionProblem);
    }
    public void AddExamination(ExaminationId examinationId, string? textResult, string? imgResult){
        var questionexamination = new QuestionExamination(
            new QuestionExaminationId(Guid.NewGuid()),
            Id,
            examinationId,
            textResult,
            imgResult
        );
        _examinations.Add(questionexamination);
    }

    public void RemoveExamination(QuestionExamination questionExamination){
        _examinations.Remove(questionExamination);
    }
    public void AddDiagnostic(Diagnostic diagnostic){
        _diagnostics.Add(diagnostic);
    }

    public void RemoveDiagnostic(Diagnostic diagnostic){
        _diagnostics.Remove(diagnostic);
    }
    public void AddTreatment(Treatment treatment){
        _treatments.Add(treatment);
        
    }
    public void RemoveTreatment(Treatment treatment){
        _treatments.Remove(treatment);
        
    }
    public void AddTag(Tag tag){
        _tags.Add(tag);
    }
    public void RemoveTag(Tag tag){
        _tags.Remove(tag);
    }
    public void AddUser(string userId){
        var questionlog = new QuestionLog(
            new QuestionLogId(Guid.NewGuid()),
            Id,
            userId,
            DateTime.UtcNow
        );
        _logs.Add(questionlog);
    }

    public void UpdateModified(bool modified){
        Modified = modified;
    }

};