// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp.IO;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;

namespace SixLabors.ImageSharp.Formats.Jpeg
{
    /// <summary>
    /// Image decoder for generating an image out of a jpg stream.
    /// </summary>
    public sealed class JpegDecoder : IImageDecoder, IJpegDecoderOptions, IImageInfoDetector
    {
        /// <inheritdoc/>
        public bool IgnoreMetadata { get; set; }

        /// <inheritdoc/>
        public Image<TPixel> Decode<TPixel>(Configuration configuration, Stream stream)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            Guard.NotNull(stream, nameof(stream));

            using var decoder = new JpegDecoderCore(configuration, this);
            return decoder.Decode<TPixel>(stream);
        }

        /// <inheritdoc />
        public Image Decode(Configuration configuration, Stream stream)
            => this.Decode<Rgba32>(configuration, stream);

        /// <inheritdoc/>
        public Task<Image<TPixel>> DecodeAsync<TPixel>(Configuration configuration, Stream stream, CancellationToken cancellationToken)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            Guard.NotNull(stream, nameof(stream));

            using var decoder = new JpegDecoderCore(configuration, this);
            return decoder.DecodeAsync<TPixel>(stream, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Image> DecodeAsync(Configuration configuration, Stream stream, CancellationToken cancellationToken)
            => await this.DecodeAsync<Rgba32>(configuration, stream, cancellationToken).ConfigureAwait(false);

        /// <inheritdoc/>
        public IImageInfo Identify(Configuration configuration, Stream stream)
        {
            Guard.NotNull(stream, nameof(stream));

            using var decoder = new JpegDecoderCore(configuration, this);
            return decoder.Identify(stream);
        }

        /// <inheritdoc/>
        public Task<IImageInfo> IdentifyAsync(Configuration configuration, Stream stream, CancellationToken cancellationToken)
        {
            Guard.NotNull(stream, nameof(stream));

            using var decoder = new JpegDecoderCore(configuration, this);
            return decoder.IdentifyAsync(stream, cancellationToken);
        }
    }
}
