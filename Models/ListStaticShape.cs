using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotLiquid;
using Orchard;

namespace Lombiq.LiquidMarkup.Models
{
    // We need to implement IList since that is only what DotLiquid understands.
    public class ListStaticShape : StaticShapeBase, IList
    {
        private readonly WorkContext _workContext;

        public object this[int index]
        {
            get
            {
                return new StaticShape(_shape, _workContext)[index];
            }
            set
            {
                throw new NotSupportedException("Collections used in Liquid cannot be assigned to.");
            }
        }


        public ListStaticShape(dynamic shape, WorkContext workContext)
        {
            _workContext = workContext;
            Initalize(shape);
        }


        #region Unused IList members
        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerator GetEnumerator()
        {
            if (!(_shape is IEnumerable))
            {
                throw new InvalidOperationException("The given shape is not an enumerable and thus can't be enumerated.");
            }

            return new StaticShapeEnumerator(((IEnumerable)_shape).GetEnumerator(), _workContext);
        }
        #endregion
    }
}