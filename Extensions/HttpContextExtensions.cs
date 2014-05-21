using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;

namespace System.Web
{
    internal static class HttpContextExtensions
    {
        public static WorkContext GetWorkContext(this HttpContext httpContext)
        {
            if (httpContext == null) return null;
            return HttpContext.Current.Request.RequestContext.GetWorkContext();
        }
    }
}