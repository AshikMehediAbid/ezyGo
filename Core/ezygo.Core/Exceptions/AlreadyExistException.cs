using System.Net;

namespace ezyGo.Core.Exceptions;

public class AlreadyExistException : Exception
{
    public HttpStatusCode HttpStatusCode { get; } = HttpStatusCode.OK;
    public string Title { get; } = "Already";

    public AlreadyExistException(string entityName) : base($"{entityName} already exist")
    {
    }

    public AlreadyExistException() : base("This info already exist")
    {
    }
}
