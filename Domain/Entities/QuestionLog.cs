namespace Domain.Entities;
public class QuestionLog{
    internal QuestionLog(){
        
    }
    public QuestionLogId Id;
    public QuestionId QuestionId;
    public string UserId;
    public DateTime DateTime;

    internal QuestionLog(QuestionLogId id, QuestionId questionId, string userId, DateTime dateTime){
        Id = id;
        QuestionId = questionId;
        UserId = userId;
        DateTime = dateTime;
    }
}