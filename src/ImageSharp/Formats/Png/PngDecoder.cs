// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp.IO;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;

namespace SixLabors.ImageSharp.Formats.Png
{
    /// <summary>
    /// Decoder for generating an image out of a png encoded stream.
    /// </summary>
    public sealed class PngDecoder : IImageDecoder, IPngDecoderOptions, IImageInfoDetector
    {
        /// <inheritdoc/>
        public bool IgnoreMetadata { get; set; }

        /// <inheritdoc/>
        public Image<TPixel> Decode<TPixel>(Configuration configuration, Stream stream)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var decoder = new PngDecoderCore(configuration, this);
            return decoder.Decode<TPixel>(stream);
        }

        /// <inheritdoc />
        public Image Decode(Configuration configuration, Stream stream) => this.Decode<Rgba32>(configuration, stream);

        /// <inheritdoc/>
        public Task<Image<TPixel>> DecodeAsync<TPixel>(Configuration configuration, Stream stream, CancellationToken cancellationToken)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var decoder = new PngDecoderCore(configuration, this);
            return decoder.DecodeAsync<TPixel>(stream, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Image> DecodeAsync(Configuration configuration, Stream stream, CancellationToken cancellationToken)
            => await this.DecodeAsync<Rgba32>(configuration, stream, cancellationToken);

        /// <inheritdoc/>
        public IImageInfo Identify(Configuration configuration, Stream stream)
        {
            var decoder = new PngDecoderCore(configuration, this);
            return decoder.Identify(stream);
        }

        /// <inheritdoc/>
        public Task<IImageInfo> IdentifyAsync(Configuration configuration, Stream stream, CancellationToken cancellationToken)
        {
            var decoder = new PngDecoderCore(configuration, this);
            return decoder.IdentifyAsync(stream, cancellationToken);
        }
    }
}
