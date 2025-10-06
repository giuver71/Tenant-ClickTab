using ClickTab.Core.Exceptions;
using EQP.EFRepository.Core.Exceptions;
using ClickTab.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickTab.Web.MiddleWares
{
    public class ExceptionHandlerFilterMiddleware : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            int errorCode = 503;
            string errorMessage = context.Exception.Message;

            if (context.Exception is EntityConcurrencyException)
            {
                errorCode = 999;
                errorMessage = "Qualche altro utente ha modificato questo record. Vuoi ricaricare i dati con le ultime modifiche apportate? Cliccando su SI perderai tutti i dati che hai modificato, se clicchi su NO resterai su questa pagina ma non si potr\u00E0 procedere col salvataggio";
            }
            else if (context.Exception is EQPTranslatedException)
            {
                errorCode = 998;
            }

            ErrorResponseObject errorResponseObject = new ErrorResponseObject()
            {
                message = errorMessage,
                stack = context.Exception.ToString(),
            };

            //Traccio il log su Serilog
            Log.Error(context.Exception, errorMessage);

            context.HttpContext.Response.StatusCode = errorCode;
            context.Result = new JsonResult(errorResponseObject);

            base.OnException(context);
        }
    }
}
