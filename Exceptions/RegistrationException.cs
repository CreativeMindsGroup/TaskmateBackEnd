using System.Net;
using TaskMate.Exceptions.CommonException;

namespace TaskMate.Exceptions;

public class RegistrationException : Exception, IBaseException
{
    public int StatusCode { get; set; }

    public string CustomMessage { get; set; }

    public RegistrationException(string message) : base(message)
    {
        CustomMessage = message;
        StatusCode = (int)HttpStatusCode.BadRequest;
    }
}