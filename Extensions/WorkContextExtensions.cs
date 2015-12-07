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
            // Logging the tenant name, because the logger can't always find it out (the URL is also logged by the logger).
            wc
                .Resolve<ILoggerFactory>()
                .CreateLogger(callerType)
                .Error(message + " Tenant: " + wc.Resolve<ShellSettings>().Name);
        }
    }
}