using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CoreTorrent.Common.Bencoding;
using Xunit;

namespace CoreTorrent.Tests.Unit.Common.Bencoding
{
    public sealed class BDecoderTests
    {
        public sealed class Decode
        {
            [Fact]
            public async Task Should_Throw_Exception_When_Stream_Is_Null()
            {
                // Given
                var decoder = new BDecoder();

                // When
                var result = await Record.ExceptionAsync(() => decoder.DecodeAsync(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
            }

            [Theory]
            [InlineData("3:foo", "foo")]
            [InlineData("26:abcdefghijklmnopqrstuvwxyz", "abcdefghijklmnopqrstuvwxyz")]
            public async Task Should_Decode_String(string encoded, string decoded)
            {
                // Given
                var decoder = new BDecoder();

                // When
                var result = await decoder.DecodeAsync(encoded);

                // Then
                Assert.IsType<BString>(result);
                Assert.Equal(decoded, ((BString) result).ToString());
            }

            [Theory]
            [InlineData("i127e", 127)]
            [InlineData("i-123e", -123)]
            [InlineData("i3294967296e", 3294967296)]
            public async Task Should_Decode_Number(string encoded, long decoded)
            {
                // Given
                var decoder = new BDecoder();

                // When
                var result = await decoder.DecodeAsync(encoded);

                // Then
                Assert.IsType<BNumber>(result);
                Assert.Equal(decoded, ((BNumber) result).Value);
            }

            [Theory]
            [InlineData("le", 0)]
            [InlineData("llelee", 2)]
            [InlineData("l4:spame", 1)]
            [InlineData("l4:spam3:ham5:foobai4ee", 4)]
            public async Task Should_Decode_List(string encoded, int expectedItems)
            {
                // Given
                var decoder = new BDecoder();

                // When
                var result = await decoder.DecodeAsync(encoded);

                // Then
                Assert.IsType<BList>(result);
                Assert.Equal(expectedItems, ((BList) result).Count);
            }

            [Theory]
            [InlineData("de", 0)]
            [InlineData("d3:hami3ee", 1)]
            [InlineData("d3:hami3e4:spamlleleee", 2)]
            public async Task Should_Decode_Dictionary(string encoded, int expectedItems)
            {
                // Given
                var decoder = new BDecoder();

                // When
                var result = await decoder.DecodeAsync(encoded);

                // Then
                Assert.IsType<BDictionary>(result);
                Assert.Equal(expectedItems, ((BDictionary) result).Count);
            }

            [Fact]
            public async Task Should_Decode_Zero_Length_String()
            {
                // Given
                var decoder = new BDecoder();

                // When
                var result = await decoder.DecodeAsync("0:");

                // Then
                Assert.IsType<BString>(result);
                Assert.NotNull(((BString) result).Value);
                Assert.Empty(((BString) result).Value);
            }

            [Fact]
            public async Task Should_Leave_Stream_At_Expected_Position()
            {
                // Given
                var data = Encoding.UTF8.GetBytes("prefix 4:spam suffix");

                using (var stream = new MemoryStream(data))
                {
                    // When
                    stream.Seek(7, SeekOrigin.Begin);
                    await new BDecoder().DecodeAsync(stream);

                    // Then
                    Assert.Equal(13, stream.Position);
                }
            }
        }
    }
}
