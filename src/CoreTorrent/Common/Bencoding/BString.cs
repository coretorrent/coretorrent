using System;

namespace CoreTorrent.Common.Bencoding
{
    public sealed class BString : BValue
    {
        public string Value { get; set; }
    }
}
