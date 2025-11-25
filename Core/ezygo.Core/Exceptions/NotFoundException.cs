using System.Net;
using System.Reflection;

namespace ezyGo.Core.Exceptions;

public class NotFoundException : Exception
{
    public HttpStatusCode HttpStatusCode { get; } = HttpStatusCode.NotFound;
    public string Title { get; } = "Not Found";

    public NotFoundException(string entityName) : base($"{entityName} Not Found!")
    {
    }

    public NotFoundException() : base("Not Found")
    {
    }

}
