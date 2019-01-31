using System;

namespace CoreTorrent.Common.Bencoding
{
    public sealed class BNumber : BValue
    {
        public long Value { get; set; }
    }
}
