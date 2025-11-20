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
        return statusCode switch
        {
            HttpStatusCode.NotFound => "Recurso no encontrado.",
            HttpStatusCode.BadRequest => await HttpResponseMessage.Content.ReadAsStringAsync(),
            HttpStatusCode.Unauthorized => "Tienes que iniciar sesión para realizar esta operación.",
            HttpStatusCode.Forbidden => "No tienes permiso para realizar esta operación.",
            _ => "Ha ocurrido un error inesperado.",
        };
    }
}