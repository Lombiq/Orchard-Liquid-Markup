using System.IO;
using System.Web.Mvc;
using Orchard.Environment.Extensions;

namespace Lombiq.LiquidMarkup.Services.ViewEngine
{
    [OrchardFeature("Lombiq.LiquidMarkup.ViewEngine")]
    public class LiquidView : IView
    {
        private readonly ILiquidTemplateService _liquidTemplateService;

        public string ViewPath { get; private set; }
        public string MasterPath { get; private set; }


        public LiquidView(ILiquidTemplateService liquidTemplateService, string viewPath, string masterPath)
        {
            _liquidTemplateService = liquidTemplateService;
            ViewPath = viewPath;
            MasterPath = masterPath;
        }


        public void Render(ViewContext context, TextWriter writer)
        {
            var filename = context.HttpContext.Server.MapPath(ViewPath);
            var output = _liquidTemplateService.ExecuteTemplate(File.ReadAllText(filename), context.ViewData.Model);
            writer.Write(output);
        }
    }
}