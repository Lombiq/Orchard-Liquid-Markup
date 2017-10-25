using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web;
using DotLiquid;
using Lombiq.LiquidMarkup.Services;
using Microsoft.CSharp.RuntimeBinder;
using Orchard;
using Orchard.DisplayManagement.Shapes;
using Orchard.Localization;
using Orchard.Validation;

namespace Lombiq.LiquidMarkup.Models
{
    // Similar in idea to the StaticShape class in OrchardHUN.Scripting.Php
    public class StaticShape : StaticShapeBase, IIndexable
    {
        private readonly WorkContext _workContext;

        public dynamic Shape { get { return _shape; } }
        public ShapeMetadata Metadata { get { return _shape.Metadata; } }
        public dynamic Id { get { return _shape.Id; } } // Depending on the shape the Id can be an int or string too.
        public IList<string> Classes { get { return _shape.Classes; } }
        public IDictionary<string, string> Attributes { get { return _shape.Attributes; } }
        private readonly Lazy<IEnumerable<dynamic>> _itemsLazy;
        public IEnumerable<dynamic> Items { get { return _itemsLazy.Value; } }


        public StaticShape(dynamic shape, WorkContext workContext)
        {
            _workContext = workContext;
            Initalize(shape);

            _itemsLazy = new Lazy<IEnumerable<dynamic>>(() =>
            {
                var items = new List<StaticShape>();
                foreach (var item in _shape.Items)
                {
                    items.Add(new StaticShape(item, workContext));
                }
                return items;
            });
        }


        public bool ContainsKey(object key)
        {
            return true;
        }

        public object this[object key]
        {
            get
            {
                var keyString = key.ToString();

                // Is key referring to a property on this class?
                // The Item property will be found as a property of this object since it is reserved for the indexer.
                // It has to be skipped otherwise the Item property of the wrapped Shape won't be available.
                // See: https://msdn.microsoft.com/en-us/library/1y4s51k3(v=vs.110).aspx#Anchor_2
                if (keyString != "Item" && typeof(StaticShape).GetProperties().Any(property => property.Name == keyString))
                {
                    return typeof(StaticShape).GetProperty(keyString).GetValue(this, null);
                }

                dynamic item = null;
                if (!(_shape is Shape))
                {
                    Type shapeType = _shape.GetType();

                    // Is key referring to a property on _shape?
                    if (shapeType.GetProperties().Any(property => property.Name == keyString))
                    {
                        item = shapeType.GetProperty(keyString).GetValue(_shape, null);
                    }
                    else
                    {
                        var indexer = shapeType.GetProperties()
                            .Where(p => p.GetIndexParameters().Length != 0)
                            .FirstOrDefault();

                        if (indexer != null)
                        {
                            // Does _shape has an indexer for key?
                            object[] indexArgs = { key };
                            item = indexer.GetValue(_shape, indexArgs);
                        }
                        else if (shapeType.IsArray)
                        {
                            // Is the shape an array?
                            item = _shape[(int)key];
                        }
                        else
                        {
                            // Is this a dynamic object with a dynamic property (like with Model.ContentItem.TitlePart.Title)?
                            var dynamicMetaObjectProvider = _shape as IDynamicMetaObjectProvider;
                            if (dynamicMetaObjectProvider != null)
                            {
                                // Since this is caching a delegate it needs to be an instance-local cache.
                                // This needs to be cached, otherwise on every access there would be a RuntimeBinderException,
                                // see: https://github.com/OrchardCMS/Orchard/issues/4600
                                // ICacheManager can't be used, because it would result in the following 
                                // DependencyResolutionException exception:
                                // None of the constructors found with 'Autofac.Core.Activators.Reflection.DefaultConstructorFinder' 
                                // on type 'Orchard.Caching.DefaultCacheManager' can be invoked with the available 
                                // services and parameters: Cannot resolve parameter 'System.Type component' of constructor 
                                // 'Void .ctor(System.Type, Orchard.Caching.ICacheHolder)'.
                                var compiledDelegate = _workContext
                                    .Resolve<IDynamicShapePropertyAccessDelegateCache>()
                                    .GetCachedDelegate(shapeType, keyString,
                                    () =>
                                    {
                                        var objectParameter = Expression.Parameter(typeof(object));
                                        var metaObject = dynamicMetaObjectProvider.GetMetaObject(objectParameter);
                                        var binder = (GetMemberBinder)Microsoft.CSharp.RuntimeBinder.Binder.GetMember(0, keyString, shapeType, new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(0, null) });
                                        var getMemberBinding = metaObject.BindGetMember(binder);
                                        var finalExpression = Expression.Block(Expression.Label(CallSiteBinder.UpdateLabel), getMemberBinding.Expression);
                                        var lambda = Expression.Lambda(finalExpression, objectParameter);
                                        return lambda.Compile();
                                    });

                                item = compiledDelegate.DynamicInvoke(_shape);
                            }
                        }
                    }
                }
                else
                {
                    item = _shape[key];
                }

                object liquidized;
                if (IsSimpleObject(item, out liquidized))
                {
                    return item;
                }

                // If the item is a collection then we make it indexable with an int. Otherwise wrapping e.g. a List
                // into a StaticShape would cause an error since StaticShape implements IIndexable but the key will be
                // attempted to be casted to string by DotLiquid.
                // Since Shape implements IEnumerable<object> we need to exclude it (it needs to be wrapped into a
                // StaticShape).
                if (!(item is Shape))
                {
                    var isIEnumerable = ((object)item)
                        .GetType()
                        .GetInterfaces()
                        .Any(implemenetedInterface =>
                            implemenetedInterface.IsGenericType &&
                            implemenetedInterface.GetGenericTypeDefinition() == typeof(IEnumerable<>));
                    if (isIEnumerable)
                    {
                        return new ListStaticShape(item, _workContext);
                    }
                }

                return new StaticShape(item, _workContext);
            }
        }

        public override object ToLiquid()
        {
            object liquidized;
            if (IsSimpleObject(_shape, out liquidized))
            {
                return liquidized;
            }
            else if (_shape is LocalizedString || _shape is HtmlString)
            {
                return _shape.ToString();
            }

            return base.ToLiquid();
        }


        private static bool IsSimpleObject(dynamic item, out object liquidized)
        {
            // For types that DotLiquid handles OOTB, see: https://github.com/dotliquid/dotliquid/blob/master/src/DotLiquid/Context.cs#L424
            if (item == null)
            {
                liquidized = null;
                return true;
            }
            else if (item is bool || item is string)
            {
                liquidized = item;
                return true;
            }
            else if (item.GetType().IsPrimitive ||
                item is decimal ||
                item is DateTime)
            {
                liquidized = item.ToString();
                return true;
            }

            liquidized = null;
            return false;
        }
    }
}