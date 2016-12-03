using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.LiquidMarkup.Services
{
    public class DefaultTemplateItemProvidedPathResolver : ITemplateItemProvidedPathResolver
    {
        public string GenerateUrlFromPath(string relativePath)
        {
            throw new NotImplementedException("Special path handling should be added in an ITemplateItemProvidedPathResolver implementation.");
        }

        public bool IsRealVirtualPath(string virtualPath)
        {
            return true; // Can't detect anything special by default.
        }
    }
}