using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CoreTorrent.Common.Bencoding
{
    public sealed class BDecoder
    {
        private sealed class PeekableReader
        {
            private readonly Stream _wrappedStream;
            private int? _peekedByte;

            public PeekableReader(Stream wrappedStream)
            {
                _wrappedStream = wrappedStream ?? throw new ArgumentNullException(nameof(wrappedStream));
            }

            public int ReadByte()
            {
                if (_peekedByte.HasValue)
                {
                    var value = _peekedByte.Value;
                    _peekedByte = null;
                    return value;
                }

                return _wrappedStream.ReadByte();
            }

            public int PeekByte()
            {
                if (_peekedByte.HasValue)
                {
                    return _peekedByte.Value;
                }

                _peekedByte = ReadByte();

                return _peekedByte.Value;
            }
        }

        const int BufferSize = 1024;

        public Task<BValue> DecodeAsync(Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var reader = new PeekableReader(stream);

            return ReadValueAsync(reader);
        }

        private static async Task<BValue> ReadValueAsync(PeekableReader reader)
        {
            var firstChar = reader.PeekByte();

            if (firstChar < 0)
            {
                throw new Exception("Unexpected end of stream.");
            }

            switch (firstChar)
            {
                case 'd':
                    return await ReadDictionaryAsync(reader);

                case 'i':
                    return await ReadNumberAsync(reader);

                case 'l':
                    return await ReadListAsync(reader);

                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '0':
                    return await ReadStringAsync(reader);
            }

            throw new NotImplementedException();
        }

        private static async Task<BDictionary> ReadDictionaryAsync(PeekableReader reader)
        {
            Expect(reader, 'd');

            var dict = new BDictionary();

            while (reader.PeekByte() != 'e')
            {
                var key = await ReadStringAsync(reader);
                var val = await ReadValueAsync(reader);

                dict.Add(key, val);
            }

            Expect(reader, 'e');

            return dict;
        }

        private static async Task<BList> ReadListAsync(PeekableReader reader)
        {
            Expect(reader, 'l');

            var list = new BList();

            while (reader.PeekByte() != 'e')
            {
                list.Add(await ReadValueAsync(reader));
            }

            Expect(reader, 'e');

            return list;
        }

        private static Task<BNumber> ReadNumberAsync(PeekableReader reader)
        {
            Expect(reader, 'i');

            var builder = new StringBuilder();

            while (reader.PeekByte() != 'e')
            {
                builder.Append((char) reader.ReadByte());
            }

            Expect(reader, 'e');

            var num = Convert.ToInt64(builder.ToString());

            return Task.FromResult(new BNumber(num));
        }

        private static Task<BString> ReadStringAsync(PeekableReader reader)
        {
            var lengthBuilder = new StringBuilder();

            while (reader.PeekByte() != ':')
            {
                lengthBuilder.Append((char) reader.ReadByte());
            }

            Expect(reader, ':');

            var length = Convert.ToInt32(lengthBuilder.ToString());
            var buffer = new byte[length];

            for (var i = 0; i < length; i++)
            {
                var b = reader.ReadByte();

                if (b < 0)
                {
                    throw new Exception("Unexpected end of stream");
                }

                buffer[i] = (byte) b;
            }

            return Task.FromResult(new BString(buffer));
        }

        private static void Expect(PeekableReader reader, char expectedChar)
        {
            var foundChar = reader.ReadByte();

            if (foundChar < 0)
            {
                throw new Exception("Unexpected end of stream");
            }

            if (foundChar != expectedChar)
            {
                throw new Exception("Unexpected character: " + foundChar);
            }
        }
    }
}
