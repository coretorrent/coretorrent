using System;
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
                Assert.Equal(decoded, ((BString)result).Value);
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
                Assert.Equal(decoded, ((BNumber)result).Value);
            }
        }
    }
}
