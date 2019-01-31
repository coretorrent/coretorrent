using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CoreTorrent.Common.Bencoding
{
    public static class BDecoderExtensions
    {
        public static Task<BValue> DecodeAsync(this BDecoder decoder, string data)
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            return decoder.DecodeAsync(buffer);
        }

        public static Task<BValue> DecodeAsync(this BDecoder decoder, byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                return decoder.DecodeAsync(stream);
            }
        }
    }
}
