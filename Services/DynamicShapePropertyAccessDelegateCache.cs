using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.LiquidMarkup.Services
{
    public class DynamicShapePropertyAccessDelegateCache : IDynamicShapePropertyAccessDelegateCache
    {
        private readonly ConcurrentDictionary<string, Delegate> _cache = new ConcurrentDictionary<string, Delegate>();


        public Delegate GetCachedDelegate(Type shapeType, string propertyName, Func<Delegate> factory) =>
            _cache.GetOrAdd(shapeType.FullName + "." + propertyName, key => factory());
    }
}