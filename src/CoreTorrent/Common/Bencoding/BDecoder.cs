using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CoreTorrent.Common.Bencoding
{
    public sealed class BDecoder
    {
        const int BufferSize = 1024;

        public Task<BValue> DecodeAsync(Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            return ReadValue(stream);
        }

        private static Task<BValue> ReadValue(Stream stream)
        {
            var firstChar = stream.ReadByte();

            if (firstChar < 0)
            {
                throw new Exception("Unexpected end of stream.");
            }

            throw new NotImplementedException();
        }
    }
}
