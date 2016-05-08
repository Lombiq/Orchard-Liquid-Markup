using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotLiquid;
using Orchard.Validation;

namespace Lombiq.LiquidMarkup.Models
{
    public abstract class StaticShapeBase : ILiquidizable
    {
        protected dynamic _shape;


        /// <summary>
        /// A quasi-constructor. We can't have a constructor that expects a dynamic argument because otherwise we'd get
        /// the following error: "The constructor call needs to be dynamically dispatched, but cannot be because it is 
        /// part of a constructor initializer. Consider casting the dynamic arguments."
        /// </summary>
        protected void Initalize(dynamic shape)
        {
            Argument.ThrowIfNull(shape, "shape");

            _shape = shape;
        }


        public object ToLiquid()
        {
            return this;
        }
    }
}