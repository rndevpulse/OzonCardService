namespace OzonCard.Common.Application.Common;

public abstract class MemberInfo
{
    public Guid UserId { get; set; }
    public void SetUserId(Guid userId) => UserId = userId;
    public string User { get; set; } = "";
    public void SetUser(string user) => User = user;
    
    public Guid? Tracking { get; set; }

    public Guid UseTracking()
    {
        var tracking  = Guid.NewGuid();
        Tracking = tracking;
        return tracking;
    } 

}