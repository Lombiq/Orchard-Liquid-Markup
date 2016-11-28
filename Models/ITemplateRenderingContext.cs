using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lombiq.LiquidMarkup.Models
{
    public enum TemplateType
    {
        TemplateFile,
        TemplateContentItem
    }


    public interface ITemplateRenderingContext
    {
        TemplateType TemplateType { get; }
        string TemplatePath { get; }
    }
}
