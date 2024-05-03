namespace OzonCard.Common.Core.Exceptions;

public class BusinessException : Exception
{
    public int Code { get; }

    public BusinessException(string? message, int code = 0, Exception? inner = null) : base(message, inner)
    {
        Code = code;
    }
}