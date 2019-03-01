using System.Collections;
using System.Collections.Generic;

namespace CoreTorrent.Common.Bencoding
{
    public sealed class BDictionary : BValue, IDictionary<BString, BValue>
    {
        private readonly IDictionary<BString, BValue> _internalDictionary;

        public BDictionary()
        {
            _internalDictionary = new Dictionary<BString, BValue>();
        }

        public BValue this[BString key] { get => _internalDictionary[key]; set => _internalDictionary[key] = value; }

        public ICollection<BString> Keys => _internalDictionary.Keys;

        public ICollection<BValue> Values => _internalDictionary.Values;

        public int Count => _internalDictionary.Count;

        public bool IsReadOnly => _internalDictionary.IsReadOnly;

        public void Add(BString key, BValue value) => _internalDictionary.Add(key, value);

        public void Add(KeyValuePair<BString, BValue> item) => _internalDictionary.Add(item);

        public void Clear() => _internalDictionary.Clear();

        public bool Contains(KeyValuePair<BString, BValue> item) => _internalDictionary.Contains(item);

        public bool ContainsKey(BString key) => _internalDictionary.ContainsKey(key);

        public void CopyTo(KeyValuePair<BString, BValue>[] array, int arrayIndex) => _internalDictionary.CopyTo(array, arrayIndex);

        public IEnumerator<KeyValuePair<BString, BValue>> GetEnumerator() => _internalDictionary.GetEnumerator();

        public bool Remove(BString key) => _internalDictionary.Remove(key);

        public bool Remove(KeyValuePair<BString, BValue> item) => _internalDictionary.Remove(item);

        public bool TryGetValue(BString key, out BValue value) => _internalDictionary.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) _internalDictionary).GetEnumerator();
    }
}
