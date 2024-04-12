using OzonCard.Common.Domain.Abstractions;

namespace OzonCard.Common.Domain.Files;

public class File : AggregateRoot
{
    public string Format { get; private set; }
    public string Name { get; private set; }
    public Guid User { get; private set; }

    public File(Guid id, string format, string name, Guid user) : base(id)
    {
        Format = format;
        Name = name;
        User = user;
    }
}