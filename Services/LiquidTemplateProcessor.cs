﻿using Orchard.DisplayManagement.Implementation;
using Orchard.Environment.Extensions;
using Orchard.Exceptions;
using Orchard.Logging;
using Orchard.Reports;
using Orchard.Reports.Services;
using Orchard.Templates.Services;
using System;
using System.Linq;

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


        public string Type
        {
            get { return "Liquid"; }
        }


        public LiquidTemplateProcessor(ILiquidTemplateService templateService, IReportsManager reportsManager)
        {
            _templateService = templateService;
            _reportsManager = reportsManager;

            Logger = NullLogger.Instance;
        }


        public string Process(string template, string name, DisplayContext context = null, dynamic model = null)
        {
            try
            {
                return _templateService.ExecuteTemplate(template, model);
            }
            catch (Exception ex)
            {
                if (ex.IsFatal()) throw;

                var errorMessage = "An unexpected exception was caught during rendering \"" + name + "\" Liquid template.";
                Logger.Error(ex, errorMessage);

                var reportActivityName = "LiquidTemplateErrors-" + name;

                var liquidReport = _reportsManager
                    .GetReports().FirstOrDefault(report => report.ActivityName == reportActivityName);

                var liquidReportId = liquidReport == null
                    ? _reportsManager.CreateReport("Liquid template errors - " + name, reportActivityName)
                    : liquidReport.ReportId;

                _reportsManager.Add(liquidReportId, ReportEntryType.Error, ex.Message);

                return errorMessage + " " + ex.Message;
            }
        }

        public void Verify(string template)
        {
            _templateService.VerifySource(template);
        }
    }
}