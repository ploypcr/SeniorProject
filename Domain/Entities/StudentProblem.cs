namespace Domain.Entities;

public class StudentProblem{
    internal StudentProblem(){

    }
    public StudentProblemId Id {get; private set;}
    public StudentStatsId StudentStatsId {get; private set;}
    public ProblemId ProblemId {get; private set;}
    public int Round {get; private set;}

    internal StudentProblem(StudentProblemId studentProblemId, StudentStatsId studentStatsId, ProblemId problemId, int round){
        Id = studentProblemId;
        StudentStatsId = studentStatsId;
        ProblemId = problemId;
        Round = round;
    }
}