using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotLiquid;
using Orchard.DisplayManagement.Shapes;

namespace Lombiq.LiquidMarkup.Models
{
    // Similar in idea to the StaticShape class in OrchardHUN.Scripting.Php
    public class StaticShape : IIndexable, ILiquidizable
    {
        private readonly dynamic _shape;
        public dynamic Shape { get { return _shape; } }
        public ShapeMetadata Metadata { get { return _shape.Metadata; } }
        public string Id { get { return _shape.Id; } }
        public IList<string> Classes { get { return _shape.Classes; } }
        public IDictionary<string, string> Attributes { get { return _shape.Attributes; } }
        public IEnumerable<dynamic> Items { get { return _shape.Items; } }


        public StaticShape(dynamic shape)
        {
            _shape = shape;
        }


        public bool ContainsKey(object key)
        {
            return true;
        }

        public object this[object key]
        {
            get
            {
                if (typeof(StaticShape).GetProperties().Any(property => property.Name == key.ToString()))
                {
                    return typeof(StaticShape).GetProperty(key.ToString()).GetValue(this, null);
                }

                dynamic item = null;
                if (!(_shape is Shape))
                {
                    Type shapeType = _shape.GetType();
                    if (shapeType.GetProperties().Any(property => property.Name == key.ToString()))
                    {
                        item = shapeType.GetProperty(key.ToString()).GetValue(_shape, null);
                    }
                }
                else
                {
                    item = _shape[key];
                }

                if (item == null) return string.Empty;

                if (item.GetType().IsPrimitive || item is decimal || item is string || item is DateTime)
                {
                    return item.ToString();
                }

                return new StaticShape(item);
            }
        }

        public object ToLiquid()
        {
            return this;
        }
    }
}