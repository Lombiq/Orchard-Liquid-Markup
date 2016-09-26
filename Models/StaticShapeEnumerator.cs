using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.LiquidMarkup.Models
{
    public class StaticShapeEnumerator : IEnumerator
    {
        private readonly IEnumerator _wrappedEnumerator;

        public object Current { get { return new StaticShape(_wrappedEnumerator.Current); } }


        public StaticShapeEnumerator(IEnumerator wrappedEnumerator)
        {
            _wrappedEnumerator = wrappedEnumerator;
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