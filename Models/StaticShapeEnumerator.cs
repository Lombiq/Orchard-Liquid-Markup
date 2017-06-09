using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;

namespace Lombiq.LiquidMarkup.Models
{
    public class StaticShapeEnumerator : IEnumerator
    {
        private readonly IEnumerator _wrappedEnumerator;
        private readonly WorkContext _workContext;

        public object Current { get { return new StaticShape(_wrappedEnumerator.Current, _workContext); } }


        public StaticShapeEnumerator(IEnumerator wrappedEnumerator, WorkContext workContext)
        {
            _wrappedEnumerator = wrappedEnumerator;
            _workContext = workContext;
        }


        public bool MoveNext()
        {
            return _wrappedEnumerator.MoveNext();
        }

        public void Reset()
        {
            _wrappedEnumerator.Reset();
        }
    }
}