using OzonCard.Common.Core;

namespace OzonCard.Common.Worker.Domain.Jobs;

public class Job : IWithId
{
    public Guid Id { get; }
    public DateTimeOffset CreatedAt { get; protected set; }
    public DateTimeOffset UpdatedAt { get; protected set; }
    public string Number { get; protected set; }
    public Guid User { get; protected set; }
    public string Arguments { get; set; }
    public string Status { get; set; }
    public DateTime? Closed { get; set; }
    public string? Progress { get; set; }
    public string? Result { get; set; }
    
    
    public Job(Guid id, string number, Guid user)
    {
        Id = id;
        Status = "Created";
        Number = number;
        User = user;
        Arguments = string.Empty;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}