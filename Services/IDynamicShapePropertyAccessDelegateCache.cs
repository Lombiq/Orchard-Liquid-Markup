using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orchard;

namespace Lombiq.LiquidMarkup.Services
{
    /// <summary>
    /// Caches delegates that were compiled to dynamically access properties on shape objects. See 
    /// <see cref="Models.StaticShape"/> for an explanation.
    /// </summary>
    public interface IDynamicShapePropertyAccessDelegateCache : ISingletonDependency
    {
        Delegate GetCachedDelegate(Type shapeType, string propertyName, Func<Delegate> factory);
    }
}
