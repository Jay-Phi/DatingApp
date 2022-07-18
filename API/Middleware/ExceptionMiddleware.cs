using System;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using API.Errors;

/// <summary>
/// Summary description for Class1
/// </summary>
/// 
namespace API.Middleware
{
	public class ExceptionMiddleware
	{
        //Variable gérant le comportement après l'exception
        private readonly RequestDelegate next;
        //Variable gérant les logs
        private readonly ILogger<ExceptionMiddleware> logger;
        //Variable définnisant l'environnement utilisée (dev,prod)
        private readonly IHostEnvironment env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
		{
            this.next = next;
            this.logger = logger;
            this.env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this.next(context);
            } catch (Exception ex)
            {
                //Log l'erreur
                this.logger.LogError(ex, ex.Message);
                //Ecriture de l'exception
                context.Response.ContentType = "application/json";
                //Ecriture du Status 500 du Serveur
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                //Si c'est une exception, on envoye la stack trace depuis l'écran web si on est en dev, sinon on envoye le message "Internal server Error"
                var response = this.env.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new ApiException(context.Response.StatusCode, "Internal Server Error")
                    ;

                //Send back the exception in Json
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                //Serialize the JSON Response
                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }

}
