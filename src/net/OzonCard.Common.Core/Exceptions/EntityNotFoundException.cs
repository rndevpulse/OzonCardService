namespace OzonCard.Common.Core.Exceptions;

public class EntityNotFoundException : Exception
{
    public string Type { get; }
    public object Id { get; }

    public EntityNotFoundException(string type, object id, string? message = null, Exception? innerException = null)
        : base(message ?? $"Entity {type} with id {id} not found", innerException)
    {
        Type = type;
        Id = id;
    }

    public static EntityNotFoundException For<T>(object id) => new(typeof(T).Name, id);
}