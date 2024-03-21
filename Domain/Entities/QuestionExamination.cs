namespace Domain.Entities;

public class QuestionExamination{

    internal QuestionExamination(){
        
    }
    public QuestionExaminationId Id {get; private set; }
    public QuestionId QuestionId {get; private set; }
    public ExaminationId ExaminationId {get; private set; }
    public string? TextResult {get; private set; }
    public string? ImgResult {get; private set; }
    internal QuestionExamination(QuestionExaminationId id, QuestionId questionId, ExaminationId examinationId, string textResult, string imgResult){
        Id = id;
        QuestionId = questionId;
        ExaminationId = examinationId;
        TextResult = textResult;
        ImgResult = imgResult;
    }
}