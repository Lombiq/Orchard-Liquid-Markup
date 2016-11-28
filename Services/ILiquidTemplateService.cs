using Lombiq.LiquidMarkup.Models;
using Orchard;

namespace Lombiq.LiquidMarkup.Services
{
    /// <summary>
    /// Generic service for dealing with Liquid-formatted templates.
    /// </summary>
    public interface ILiquidTemplateService : IDependency
    {
        /// <summary>
        /// Runs a template from source.
        /// </summary>
        /// <param name="liquidSource">The Liquid-formatted source code.</param>
        /// <param name="model">Contextual model for the the template.</param>
        /// <param name="templateRenderingContext">Contextual information about the template rendering.</param>
        /// <returns>The rendered markup.</returns>
        string ExecuteTemplate(string liquidSource, dynamic model, ITemplateRenderingContext templateRenderingContext);

        /// <summary>
        /// Verifies if the Liquid source code is correct. Throws exceptions on error.
        /// </summary>
        /// <param name="liquidSource">The Liquid-formatted source code.</param>
        void VerifySource(string liquidSource);
    }
}
