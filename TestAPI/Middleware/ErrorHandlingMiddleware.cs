using TestNetAPI.Exceptions;

namespace TestNetAPI.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware (ILogger<ErrorHandlingMiddleware> logger) //zaciągamy logger zeby w razie wystąpienia wyjątku miec z tego logi
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next) // obsługa wyjątków w całym api
        {           
           try
            {
                await next.Invoke(context);
            }
            catch(BadRequestException badRequestException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(badRequestException.Message);
            }
            catch (NotFoundException notFoundException)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(notFoundException.Message); 
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                context.Response.StatusCode = 500;  //generujemy kod błedu i wiadomość dla użytkownika 
                await context.Response.WriteAsync("Coś poszło nie tak ");
            }
        }
    }
}
