using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotLiquid;
using Orchard.Caching.Services;
using Orchard.DisplayManagement.Implementation;
using Orchard.Environment.Extensions;
using Orchard.Templates.Services;

namespace Lombiq.LiquidMarkup.Services
{
    [OrchardFeature("Lombiq.LiquidMarkup.Templates")]
    public class LiquidTemplateProcessor : ITemplateProcessor
    {
        private readonly ICacheService _cacheService;


        public string Type
        {
            get { return "Liquid"; }
        }


        public LiquidTemplateProcessor(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }
        

        public string Process(string template, string name, DisplayContext context = null, dynamic model = null)
        {
            // Currently only global configuration is possible, see: https://github.com/formosatek/dotliquid/issues/93
            Template.NamingConvention = new DotLiquid.NamingConventions.CSharpNamingConvention();

            var liquidTemplate = _cacheService.Get(template, () => Template.Parse(template));
            return liquidTemplate.Render(new RenderParameters
            {
                RethrowErrors = true
            });
        }

        public void Verify(string template)
        {
            Template.Parse(template);
        }
    }
}