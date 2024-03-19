namespace TaskMate.Exceptions.CommonException;

public interface IBaseException
{
    int StatusCode { get; }
    string CustomMessage { get; }
}