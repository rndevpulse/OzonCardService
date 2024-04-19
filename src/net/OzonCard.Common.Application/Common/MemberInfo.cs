namespace OzonCard.Common.Application.Common;

public abstract class MemberInfo
{
    public Guid UserId { get; private set; }
    public void SetUserId(Guid userId) => UserId = userId;
    public string User { get; private set; } = "";
    public void SetUser(string user) => User = user;
    
    public Guid TaskId { get; private set; }
    public void SetTaskId(Guid taskId) => TaskId = taskId;
}