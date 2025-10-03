using System.Net;


namespace Share.Common.Exceptions
{
    public abstract class HttpException(string message, HttpStatusCode statusCode) : Exception(message)
    {
        public HttpStatusCode StatusCode { get; set; } = statusCode;
    }

    public class HttpNotFound(string message) : HttpException(message, HttpStatusCode.NotFound)
    {

    }

    public class HttpUnauthorized(string message) : HttpException(message, HttpStatusCode.Unauthorized)
    {

    }

    public class HttpBadRequest(string message) : HttpException(message, HttpStatusCode.BadRequest)
    {

    }

    public class HttpForbidden(string message) : HttpException(message, HttpStatusCode.Forbidden)
    {

    }

    public class InternalServerError(string message) : HttpException(message, HttpStatusCode.InternalServerError)
    {

    }
}