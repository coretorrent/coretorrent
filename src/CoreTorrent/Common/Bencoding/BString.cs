using System;
using System.Text;

namespace CoreTorrent.Common.Bencoding
{
    public sealed class BString : BValue
    {
        private readonly byte[] _data;

        public BString(byte[] data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(_data);
        }

        public byte[] Value
        {
            get { return _data; }
        }
    }
}
