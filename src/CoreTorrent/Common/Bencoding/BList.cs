using System.Collections;
using System.Collections.Generic;

namespace CoreTorrent.Common.Bencoding
{
    public sealed class BList : BValue, IList<BValue>
    {
        private readonly IList<BValue> _internalList;

        public BList()
        {
            _internalList = new List<BValue>();
        }

        public BValue this[int index] { get => _internalList[index]; set => _internalList[index] = value; }

        public int Count => _internalList.Count;

        public bool IsReadOnly => _internalList.IsReadOnly;

        public void Add(BValue item)
        {
            _internalList.Add(item);
        }

        public void Clear()
        {
            _internalList.Clear();
        }

        public bool Contains(BValue item)
        {
            return _internalList.Contains(item);
        }

        public void CopyTo(BValue[] array, int arrayIndex)
        {
            _internalList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<BValue> GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        public int IndexOf(BValue item)
        {
            return _internalList.IndexOf(item);
        }

        public void Insert(int index, BValue item)
        {
            _internalList.Insert(index, item);
        }

        public bool Remove(BValue item)
        {
            return _internalList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _internalList.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _internalList).GetEnumerator();
        }
    }
}
