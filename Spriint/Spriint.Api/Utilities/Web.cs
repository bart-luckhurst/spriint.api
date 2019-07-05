using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Utilities
{
    public static class Web
    {
        public static string GetBaseUrl(HttpContext context)
        {
            HttpRequest request = context.Request;
            string host = request.Host.ToUriComponent();
            string pathBase = request.PathBase.ToUriComponent();
            return $"{request.Scheme}://{host}{pathBase}";
        }
    }
}
