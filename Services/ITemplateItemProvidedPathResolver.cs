using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orchard;

namespace Lombiq.LiquidMarkup.Services
{
    /// <summary>
    /// Service for dealing with paths generated from Liquid template content items (as opposed to template files).
    /// </summary>
    public interface ITemplateItemProvidedPathResolver : IDependency
    {
        /// <summary>
        /// Checkes whether the given virtual path is one that should actually be handled as a virtual path. Since
        /// virtual paths in template items don't necessarily need to work in the same way as standard virtual paths
        /// (like in templates deployed as part of Media Themes on DotNest, see: 
        /// https://dotnest.com/knowledge-base/topics/theming/writing-a-dotnest-theme-from-scratch) this is a check.
        /// </summary>
        /// <param name="virtualPath">A virtual path pointing to e.g. a static resource.</param>
        /// <returns>Returns <c>true</c> if the virtual path seems to be pointing to an actual resource.</returns>
        bool IsRealVirtualPath(string virtualPath);

        /// <summary>
        /// Generates a URL from a (virtual or other) relative path that is usable in a template item.
        /// </summary>
        /// <param name="relativePath">A (virtual or other) relative path.</param>
        /// <returns>A public URL that can be used to access the resource.</returns>
        string GenerateUrlFromPath(string relativePath);
    }
}
