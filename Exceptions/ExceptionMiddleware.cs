using System.Net;
using System.Text.Json;

namespace EstudoApi.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {  
            _next = next; 
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NaoEncontrouException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.ContentType = "application/json";

                var json = JsonSerializer.Serialize(new { value = ex.Message });
                await context.Response.WriteAsync(json);
            }
            catch (Exception)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var json = JsonSerializer.Serialize(new { erro = "Erro inesperado no servidor." });
                await context.Response.WriteAsync(json);
            }
        }
    }
}
