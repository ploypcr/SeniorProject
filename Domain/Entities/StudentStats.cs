using System;
namespace Domain.Entities;

public class StudentStats{
    public StudentStatsId Id {get; private set;}
    public string UserId {get; private set;}
    public QuestionId QuestionId {get; private set;}
    private readonly List<StudentExamination> _examinations = new();
    private readonly List<StudentProblem> _problems= new();
    private readonly List<Diagnostic> _diagnostics = new();
    private readonly List<Treatment> _treatments = new();

    public IReadOnlyList<StudentExamination> Examinations => _examinations.ToList();
    public IReadOnlyList<StudentProblem> Problems => _problems.ToList();
    public IReadOnlyList<Diagnostic> Diagnostics => _diagnostics.ToList();
    public IReadOnlyList<Treatment> Treatments => _treatments.ToList();
    public double QuesVersion {get; private set;}
    public double Problem1_Score {get; private set;}
    public double Problem2_Score {get; private set;}
    public double Examination_Score {get; private set;}
    public double Treatment_Score {get; private set;}
    public double Diff_Diagnostic_Score {get; private set;}
    public double Ten_Diagnostic_Score {get; private set;}
    public string? ExtraAns {get; private set;}

    public DateTime DateTime {get; private set;}
    public static StudentStats Create(
        string userId,
        QuestionId questionId,
        double quesVersion,
        string extraAns
    ){
        return new StudentStats{
            Id = new StudentStatsId(Guid.NewGuid()),
            UserId = userId,
            QuestionId = questionId,
            QuesVersion = quesVersion,
            DateTime = DateTime.UtcNow,
            ExtraAns = extraAns
        };
    }
    public void SetScore(
        double problems1_score,
        double problems2_score,
        double examinations_score,
        double treatment_score,
        double diff_diagnostic_score,
        double ten_diagnostic_score
    ){
        Problem1_Score = problems1_score;
        Problem2_Score = problems2_score;
        Examination_Score = examinations_score;
        Treatment_Score = treatment_score;
        Diff_Diagnostic_Score = diff_diagnostic_score;
        Ten_Diagnostic_Score = ten_diagnostic_score;

    }
    
    public void AddProblem(ProblemId problemId, int round){
        var studentProblem = new StudentProblem(
            new StudentProblemId(Guid.NewGuid()),
            Id,
            problemId,
            round
        );

        _problems.Add(studentProblem);
    }
        public void AddExamination(ExaminationId examinationId){
        var studentExamination = new StudentExamination(
            new StudentExaminationId(Guid.NewGuid()),
            Id,
            examinationId
        );

        _examinations.Add(studentExamination);
    }
    public void AddTreatment(Treatment treatment){

        _treatments.Add(treatment);
    }

    public void AddDiagnostic(Diagnostic diagnostic){
        _diagnostics.Add(diagnostic);
    }
}