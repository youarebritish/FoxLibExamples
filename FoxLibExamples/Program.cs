namespace FoxLibExamples
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using PowerCutAreaGimmickLocatorSet = FoxLib.Tpp.GimmickLocatorSet.GimmickLocatorSet.PowerCutAreaGimmickLocatorSet;
    using NamedGimmickLocatorSet = FoxLib.Tpp.GimmickLocatorSet.GimmickLocatorSet.NamedGimmickLocatorSet;
    using ScaledGimmickLocatorSet = FoxLib.Tpp.GimmickLocatorSet.GimmickLocatorSet.ScaledGimmickLocatorSet;
    using Vector4 = FoxLib.Core.Vector4;

    class Program
    {
        static void Main(string[] args)
        {
            MakeRandomNamedLba(20, "hello_world_001.lba");
            ReadLbaAndPrintLocatorPositions("hello_world_001.lba");
            Console.ReadLine();
        }

        /// <summary>
        /// Make a PowerCutAreaGimmickLocatorSet with random values and write it to an lba file.
        /// </summary>
        /// <param name="locatorCount">Number of locators to generate.</param>
        /// <param name="outputPath">Path of the file to write to.</param>
        private static void MakeRandomPowerCutAreaLba(int locatorCount, string outputPath)
        {
            var locatorSet = GimmickLocatorSetExamples.PowerCutArea.CreateRandomLocatorSet(locatorCount);
            GimmickLocatorSetExamples.WriteLocatorSet(locatorSet, outputPath);
        }

        /// <summary>
        /// Make a NamedGimmickLocatorSet with random values and write it to an lba file.
        /// </summary>
        /// <param name="locatorCount">Number of locators to generate.</param>
        /// <param name="outputPath">Path of the file to write to.</param>
        private static void MakeRandomNamedLba(int locatorCount, string outputPath)
        {
            var locatorSet = GimmickLocatorSetExamples.Named.CreateRandomLocatorSet(locatorCount);
            GimmickLocatorSetExamples.WriteLocatorSet(locatorSet, outputPath);
        }

        /// <summary>
        /// Make a ScaledGimmickLocatorSet with random values and write it to an lba file.
        /// </summary>
        /// <param name="locatorCount">Number of locators to generate.</param>
        /// <param name="outputPath">Path of the file to write to.</param>
        private static void MakeRandomScaledLba(int locatorCount, string outputPath)
        {
            var locatorSet = GimmickLocatorSetExamples.Scaled.CreateRandomLocatorSet(locatorCount);
            GimmickLocatorSetExamples.WriteLocatorSet(locatorSet, outputPath);
        }

        /// <summary>
        /// Read a .lba file and write the positions of its locators to the console.
        /// </summary>
        /// <param name="lbaPath">Path of the file to read.</param>
        private static void ReadLbaAndPrintLocatorPositions(string lbaPath)
        {
            var locatorSet = GimmickLocatorSetExamples.ReadLocatorSet(lbaPath);
            IEnumerable<Vector4> positions = null;

            // The locatorSet could be one of three different types.
            // Determine the type and cast it to get access to type-specific fields.
            if (locatorSet.IsPowerCutAreaGimmickLocatorSet)
            {
                var pca = locatorSet as PowerCutAreaGimmickLocatorSet;
                positions = from locator in pca.Locators
                            select locator.Position;                
            }
            else if (locatorSet.IsNamedGimmickLocatorSet)
            {
                var named = locatorSet as NamedGimmickLocatorSet;
                positions = from locator in named.Locators
                            select locator.Position;
            }
            else if (locatorSet.IsScaledGimmickLocatorSet)
            {
                var named = locatorSet as ScaledGimmickLocatorSet;
                positions = from locator in named.Locators
                            select locator.Position;
            }
            foreach (var position in positions)
            {
                Console.WriteLine(position);
            }
        }
    }
}
