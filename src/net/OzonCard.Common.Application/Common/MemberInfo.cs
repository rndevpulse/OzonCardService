﻿namespace OzonCard.Common.Application.Common;

public abstract class MemberInfo
{
    public Guid UserId { get; private set; }
    public void SetUserId(Guid userId) => UserId = userId;
    public string User { get; private set; } = "";
    public void SetUser(string user) => User = user;
    
    public Guid? Tracking { get; private set; }

    public Guid UseTracking()
    {
        var tracking  = Guid.NewGuid();
        Tracking = tracking;
        return tracking;
    } 

}