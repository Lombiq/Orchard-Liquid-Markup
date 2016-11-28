using Orchard.DisplayManagement.Implementation;
using Orchard.Environment.Extensions;
using Orchard.Exceptions;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Reports;
using Orchard.Reports.Services;
using Orchard.Templates.Services;
using System;
using System.Linq;
using Lombiq.LiquidMarkup.Models;

namespace Lombiq.LiquidMarkup.Services
{
    /// <summary>
    /// <see cref="ITemplateProcessor"/> implementation for Orchard.Templates that provides admin-editable Liquid templates.
    /// </summary>
    [OrchardFeature("Lombiq.LiquidMarkup.Templates")]
    public class LiquidTemplateProcessor : ITemplateProcessor
    {
        private readonly ILiquidTemplateService _templateService;
        private readonly IReportsManager _reportsManager;

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }


        public string Type
        {
            get { return "Liquid"; }
        }


        public LiquidTemplateProcessor(ILiquidTemplateService templateService, IReportsManager reportsManager)
        {
            _templateService = templateService;
            _reportsManager = reportsManager;

            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }


        public string Process(string template, string name, DisplayContext context = null, dynamic model = null)
        {
            try
            {
                var renderingContext = new TemplateRenderingContext
                {
                    TemplateType = TemplateType.TemplateContentItem,
                    TemplatePath = name
                };

                return _templateService.ExecuteTemplate(template, model, renderingContext);
            }
            catch (Exception ex)
            {
                if (ex.IsFatal()) throw;

                Logger.Error(ex, "An unexpected exception was caught during rendering the \"" + name + "\" Liquid template.");

                var reportActivityName = T("Liquid template errors: {0} shape", name).Text;

                var liquidReport = _reportsManager
                    .GetReports().FirstOrDefault(report => report.ActivityName == reportActivityName);

                var liquidReportId = liquidReport == null
                    ? _reportsManager
                        .CreateReport(
                            T("Errors caught in the Liquid template of the {0} shape.", name).Text,
                            reportActivityName)
                    : liquidReport.ReportId;

                _reportsManager.Add(liquidReportId, ReportEntryType.Error, ex.Message);

                return T("<strong style=\"color:red;font-weight:bold;\">An unexpected exception was caught during rendering \"{0}\" Liquid template. {1}</strong>", name, ex.Message).Text;
            }
        }

        public void Verify(string template)
        {
            _templateService.VerifySource(template);
        }
    }
}