using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// <returns>The rendered markup.</returns>
        string ExecuteTemplate(string liquidSource, dynamic model);

        /// <summary>
        /// Verifies if the Liquid source code is correct. Throws exceptions on error.
        /// </summary>
        /// <param name="liquidSource">The Liquid-formatted source code.</param>
        void VerifySource(string liquidSource);
    }
}
