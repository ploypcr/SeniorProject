namespace Domain.Entities;
public class QuestionProblem{
    internal QuestionProblem(){
    }
    public QuestionProblemId Id {get; private set;}
    public ProblemId ProblemId {get; private set;}
    public QuestionId QuestionId {get; private set;}
    public int Round {get; private set;}
    
    internal QuestionProblem(QuestionProblemId id, ProblemId problemId, QuestionId questionId, int round){
        Id = id;
        ProblemId = problemId;
        QuestionId = questionId;
        Round = round;
    }
}