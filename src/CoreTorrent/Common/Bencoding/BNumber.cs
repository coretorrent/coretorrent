using System;

namespace CoreTorrent.Common.Bencoding
{
    public sealed class BNumber : BValue
    {
        public BNumber(long value)
        {
            Value = value;
        }

        public long Value { get; set; }
    }
}
