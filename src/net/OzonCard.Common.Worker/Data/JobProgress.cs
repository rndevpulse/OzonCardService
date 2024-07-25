namespace OzonCard.Common.Worker.Data;


public record JobProgress<TProgress>(TProgress Status, object? Result);
