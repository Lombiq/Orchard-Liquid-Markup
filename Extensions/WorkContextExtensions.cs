using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.Environment.Configuration;
using Orchard.Logging;
using Orchard.Mvc;

namespace System.Web
{
    internal static class WorkContextExtensions
    {
        public static void LogSecurityNotificationWithContext(this WorkContext wc, Type callerType, string message)
        {
            var shellName = wc.Resolve<ShellSettings>().Name;
            var httpContext = wc.Resolve<IHttpContextAccessor>().Current();
            var requestUrl = httpContext != null ? httpContext.Request.Url.ToString() : "(no HTTP context)";

            wc
                .Resolve<ILoggerFactory>()
                .CreateLogger(callerType)
                .Error(message, " Tenant: " + shellName + " URL: " + requestUrl);
        }
    }
}