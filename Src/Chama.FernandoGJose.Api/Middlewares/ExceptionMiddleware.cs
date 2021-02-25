using Chama.FernandoGJose.Application.Share.Dtos;
using Chama.FernandoGJose.Util.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace Chama.FernandoGJose.Api.Middlewares
{
    public static class ExceptionMiddleware
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    IExceptionHandlerFeature contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        if (contextFeature.Error is CommandParameterException)
                        {
                            // Set HttpStatusCode
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                            // Get Error List
                            var errors = JsonConvert.DeserializeObject<List<string>>(contextFeature.Error.Message);
                            var response = new ResponseDto { Success = false, Object = errors };

                            // TODO: Log Error, using SeriLog or another one

                            // Return
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(response)).ConfigureAwait(true);
                        }
                        else
                        {
                            // TODO: Log Error, using SeriLog or another one

                            // Return 
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ResponseDto
                            {
                                Success = false,
                                Object = new List<string> { { contextFeature.Error.Message } }
                            })).ConfigureAwait(true);
                        }
                    }
                });
            });
        }
    }
}
