using Orchard.DisplayManagement.Implementation;
using Orchard.Environment.Extensions;
using Orchard.Templates.Services;

namespace Lombiq.LiquidMarkup.Services
{
    /// <summary>
    /// <see cref="ITemplateProcessor"/> implementation for Orchard.Templates that provides admin-editable Liquid templates.
    /// </summary>
    [OrchardFeature("Lombiq.LiquidMarkup.Templates")]
    public class LiquidTemplateProcessor : ITemplateProcessor
    {
        private readonly ILiquidTemplateService _templateService;


        public string Type
        {
            get { return "Liquid"; }
        }


        public LiquidTemplateProcessor(ILiquidTemplateService templateService)
        {
            _templateService = templateService;
        }
        

        public string Process(string template, string name, DisplayContext context = null, dynamic model = null)
        {
            return _templateService.ExecuteTemplate(template, model);
        }

        public void Verify(string template)
        {
            _templateService.VerifySource(template);
        }
    }
}