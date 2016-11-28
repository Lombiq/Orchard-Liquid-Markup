using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.LiquidMarkup.Models
{
    public class TemplateRenderingContext : ITemplateRenderingContext
    {
        public TemplateType TemplateType { get; set; }
        public string TemplatePath { get; set; }
    }
}