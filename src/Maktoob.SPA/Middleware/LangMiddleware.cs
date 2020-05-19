using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Maktoob.SPA.Middleware
{
    public class LangMiddleware
    {
        private readonly RequestDelegate _next;

        public LangMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var lang = context.Request.Headers["Accept-Language"]
                .ToString()
                .Split(',')
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(lang))
            {
                lang = "en";
            }
            var culture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            await _next(context);
        }
    }
}
