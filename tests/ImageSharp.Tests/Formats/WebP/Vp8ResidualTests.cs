// Copyright (c) Six Labors.
// Licensed under the Six Labors Split License.

using System.Runtime.Serialization.Formatters.Binary;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Formats.Webp.Lossy;
using SixLabors.ImageSharp.Tests.TestUtilities;

namespace SixLabors.ImageSharp.Tests.Formats.Webp;

[Trait("Format", "Webp")]
public class Vp8ResidualTests
{
    [Fact]
    public void Vp8Residual_Serialization_Works()
    {
        // arrange
        Vp8Residual expected = new();
        Vp8EncProba encProb = new();
        Random rand = new(439);
        CreateRandomProbas(encProb, rand);
        CreateCosts(encProb, rand);
        expected.Init(1, 0, encProb);
        for (int i = 0; i < expected.Coeffs.Length; i++)
        {
            expected.Coeffs[i] = (byte)rand.Next(255);
        }

        // act
        BinaryFormatter formatter = new();
        using MemoryStream ms = new();
        formatter.Serialize(ms, expected);
        ms.Position = 0;
        object obj = formatter.Deserialize(ms);
        Vp8Residual actual = (Vp8Residual)obj;

        // assert
        Assert.Equal(expected.CoeffType, actual.CoeffType);
        Assert.Equal(expected.Coeffs, actual.Coeffs);
        Assert.Equal(expected.Costs.Length, actual.Costs.Length);
        Assert.Equal(expected.First, actual.First);
        Assert.Equal(expected.Last, actual.Last);
        Assert.Equal(expected.Stats.Length, actual.Stats.Length);
        for (int i = 0; i < expected.Stats.Length; i++)
        {
            Vp8StatsArray[] expectedStats = expected.Stats[i].Stats;
            Vp8StatsArray[] actualStats = actual.Stats[i].Stats;
            Assert.Equal(expectedStats.Length, actualStats.Length);
            for (int j = 0; j < expectedStats.Length; j++)
            {
                Assert.Equal(expectedStats[j].Stats, actualStats[j].Stats);
            }
        }

        Assert.Equal(expected.Prob.Length, actual.Prob.Length);
        for (int i = 0; i < expected.Prob.Length; i++)
        {
            Vp8ProbaArray[] expectedProbabilities = expected.Prob[i].Probabilities;
            Vp8ProbaArray[] actualProbabilities = actual.Prob[i].Probabilities;
            Assert.Equal(expectedProbabilities.Length, actualProbabilities.Length);
            for (int j = 0; j < expectedProbabilities.Length; j++)
            {
                Assert.Equal(expectedProbabilities[j].Probabilities, actualProbabilities[j].Probabilities);
            }
        }

        for (int i = 0; i < expected.Costs.Length; i++)
        {
            Vp8CostArray[] expectedCosts = expected.Costs[i].Costs;
            Vp8CostArray[] actualCosts = actual.Costs[i].Costs;
            Assert.Equal(expectedCosts.Length, actualCosts.Length);
            for (int j = 0; j < expectedCosts.Length; j++)
            {
                Assert.Equal(expectedCosts[j].Costs, actualCosts[j].Costs);
            }
        }
    }

    [Fact]
    public void GetResidualCost_Works()
    {
        // arrange
        int ctx0 = 0;
        int expected = 20911;
        byte[] data = File.ReadAllBytes(Path.Combine("TestDataWebp", "Vp8Residual.bin"));
        BinaryFormatter formatter = new();
        using Stream stream = new MemoryStream(data);
        object obj = formatter.Deserialize(stream);
        Vp8Residual residual = (Vp8Residual)obj;

        // act
        int actual = residual.GetResidualCost(ctx0);

        // assert
        Assert.Equal(expected, actual);
    }

    private static void CreateRandomProbas(Vp8EncProba probas, Random rand)
    {
        for (int t = 0; t < WebpConstants.NumTypes; ++t)
        {
            for (int b = 0; b < WebpConstants.NumBands; ++b)
            {
                for (int c = 0; c < WebpConstants.NumCtx; ++c)
                {
                    for (int p = 0; p < WebpConstants.NumProbas; ++p)
                    {
                        probas.Coeffs[t][b].Probabilities[c].Probabilities[p] = (byte)rand.Next(255);
                    }
                }
            }
        }
    }

    private static void CreateCosts(Vp8EncProba probas, Random rand)
    {
        for (int i = 0; i < probas.RemappedCosts.Length; i++)
        {
            for (int j = 0; j < probas.RemappedCosts[i].Length; j++)
            {
                for (int k = 0; k < probas.RemappedCosts[i][j].Costs.Length; k++)
                {
                    ushort[] costs = probas.RemappedCosts[i][j].Costs[k].Costs;
                    for (int m = 0; m < costs.Length; m++)
                    {
                        costs[m] = (byte)rand.Next(255);
                    }
                }
            }
        }
    }

    private static void RunSetCoeffsTest()
    {
        // arrange
        Vp8Residual residual = new();
        short[] coeffs = { 110, 0, -2, 0, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0 };

        // act
        residual.SetCoeffs(coeffs);

        // assert
        Assert.Equal(9, residual.Last);
    }

    [Fact]
    public void SetCoeffsTest_Works() => FeatureTestRunner.RunWithHwIntrinsicsFeature(RunSetCoeffsTest, HwIntrinsics.AllowAll);

    [Fact]
    public void SetCoeffsTest_WithoutSSE2_Works() => FeatureTestRunner.RunWithHwIntrinsicsFeature(RunSetCoeffsTest, HwIntrinsics.DisableSSE2);
}
