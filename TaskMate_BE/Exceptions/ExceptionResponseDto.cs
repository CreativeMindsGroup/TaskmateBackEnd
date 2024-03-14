namespace TaskMate.Exceptions;

public class ExceptionResponseDto
{
    public int StatusCode { get; set; }
    public string CustomMessage { get; set; }

    public ExceptionResponseDto(int statusCode, string customMessage)
    {
        StatusCode = statusCode;
        CustomMessage = customMessage;
    }
}