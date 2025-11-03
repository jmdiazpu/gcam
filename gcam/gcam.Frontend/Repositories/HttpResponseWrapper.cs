using System.Net;

namespace gcam.Frontend.Repositories;

public class HttpResponseWrapper<T>
{
    public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
    {
        Response = response;
        Error = error;
        HttpResponseMessage = httpResponseMessage;
    }

    public T? Response { get; set; }
    public bool Error { get; set; }
    public HttpResponseMessage HttpResponseMessage { get; set; }

    public async Task<string> GetErrorMessageAsync()
    {
        if (!Error) return null;

        var statusCode = HttpResponseMessage.StatusCode;
        switch (statusCode)
        {
            case HttpStatusCode.NotFound:
                return "Recurso no encontrado.";

            case HttpStatusCode.BadRequest:
                return await HttpResponseMessage.Content.ReadAsStringAsync();

            case HttpStatusCode.Unauthorized:
                return "Tienes que iniciar sesión para realizar esta operación.";

            case HttpStatusCode.Forbidden:
                return "No tienes permiso para realizar esta operación.";

            default:
                return "Ha ocurrido un error inesperado.";
        }
    }
}