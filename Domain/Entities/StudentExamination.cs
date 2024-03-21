namespace Domain.Entities;

public class StudentExamination{
    internal StudentExamination(){

    }
    public StudentExaminationId Id {get; private set;}
    public StudentStatsId StudentStatsId {get; private set;}
    public ExaminationId ExaminationId {get; private set;}

    internal StudentExamination(StudentExaminationId studentExaminationId, StudentStatsId studentStatsId, ExaminationId examinationId){
        Id = studentExaminationId;
        StudentStatsId = studentStatsId;
        ExaminationId = examinationId;
    }
}