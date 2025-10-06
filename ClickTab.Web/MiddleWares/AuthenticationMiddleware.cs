using ClickTab.Core.Attributes;
using ClickTab.Core.DAL.Models.Generics;
using ClickTab.Core.HelperService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickTab.Web.MiddleWares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, JwtService jwtService, SessionService sessionService)
        {
            //Se l'endpoint richiesto è smarcato con l'attributo AllowAnonymous o il WebSocketAttribute allora non verifica il token 
            //e reinstrada subito la chiamata;
            Endpoint endpoint = context.GetEndpoint();
            if (endpoint != null && (endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>() != null || endpoint.Metadata.GetMetadata<WebSocketAttribute>() != null))
            {
                await _next.Invoke(context);
                return;
            }

            //Setta l'HttpContext per la gestione delle sessioni http
            sessionService.SetCurrentContext(context);

            //Se la rotta richiesta è da autenticare verifica la presenza del token e lo decodifica con JWT
            string token = context.Request.Headers["Authorization"];
            Dictionary<string, object> payload = jwtService.TokenToPayload(token);

            if (payload != null) 
            {
                User currentUser = jwtService.PayloadToUser(JsonConvert.DeserializeObject<Dictionary<string, object>>(payload["User"].ToString()));
                sessionService.SetCurrentSession(currentUser);
                await _next.Invoke(context);
                return;
            }
            else
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized Access");
            }
        }
    }

    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseEQPAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
