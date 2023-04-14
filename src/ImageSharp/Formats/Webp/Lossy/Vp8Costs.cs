// Copyright (c) Six Labors.
// Licensed under the Six Labors Split License.

namespace SixLabors.ImageSharp.Formats.Webp.Lossy;

[Serializable]
internal class Vp8Costs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Vp8Costs"/> class.
    /// </summary>
    public Vp8Costs()
    {
        this.Costs = new Vp8CostArray[WebpConstants.NumCtx];
        for (int i = 0; i < WebpConstants.NumCtx; i++)
        {
            this.Costs[i] = new Vp8CostArray();
        }
    }

    /// <summary>
    /// Gets the Costs.
    /// </summary>
    public Vp8CostArray[] Costs { get; }
}
