using LinkShortener.Domain.DTO.Request;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkShortener.Domain.Mapper
{
    public static class HttpContextMapper
    {
        public static HttpContextRequest ToHttpContextRequest(this HttpContext context)
        {
            return new HttpContextRequest
            {
                Scheme = context.Request.Scheme,
                Host = context.Request.Host.ToString(),
            };
        }
    }
}
