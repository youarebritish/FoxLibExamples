namespace FoxLibExamples
{
    using Vector4 = FoxLib.Core.Vector4;
    using Quaternion = FoxLib.Core.Quaternion;
    using WideVector3 = FoxLib.Core.WideVector3;
    using PowerCutAreaGimmickLocator = FoxLib.Tpp.GimmickLocator.PowerCutAreaGimmickLocator;
    using NamedGimmickLocator = FoxLib.Tpp.GimmickLocator.NamedGimmickLocator;
    using ScaledGimmickLocator = FoxLib.Tpp.GimmickLocator.ScaledGimmickLocator;

    using GimmickLocatorSet = FoxLib.Tpp.GimmickLocatorSet.GimmickLocatorSet;

    using System.Collections.Generic;
    using System;
    using System.IO;

    /// <summary>
    /// Examples of working with GimmickLocatorSets (.lba files).
    /// </summary>
    public static class GimmickLocatorSetExamples
    {
        /// <summary>
        /// Delegate to generate a random float.
        /// </summary>
        /// <returns>The generated float.</returns>
        private delegate float RandomFloat();

        /// <summary>
        /// Examples of woring with PowerCutAreaGimmickLocatorSets.
        /// </summary>
        public static class PowerCutArea
        {
            /// <summary>
            /// Create a PowerCutAreaGimmickLocator with random values.
            /// </summary>
            /// <param name="randomFloat">Function to generate a random float.</param>
            /// <returns>The generated locator.</returns>
            private static PowerCutAreaGimmickLocator CreateLocator(RandomFloat randomFloat)
            {
                var position = new Vector4(randomFloat(), randomFloat(), randomFloat(), randomFloat());
                var rotation = new Quaternion(randomFloat(), randomFloat(), randomFloat(), randomFloat());
                var locator = new PowerCutAreaGimmickLocator(position, rotation);
                return locator;
            }

            /// <summary>
            /// Create a PowerCutAreaGimmickLocatorSet with random values.
            /// </summary>
            /// <param name="locatorCount">Number of locators to generate.</param>
            /// <returns>The generated locatorSet.</returns>
            public static GimmickLocatorSet CreateRandomLocatorSet(int locatorCount)
            {
                var random = new Random();
                Func<PowerCutAreaGimmickLocator> createLocator = () => CreateLocator(() => GetRandomFloat(random));
                
                var locators = new List<PowerCutAreaGimmickLocator>();
                for(int i = 0; i < locatorCount; i++)
                {
                    locators.Add(createLocator());
                }

                var locatorSet = GimmickLocatorSet.NewPowerCutAreaGimmickLocatorSet(locators);
                return locatorSet;
            }
        }

        /// <summary>
        /// Examples of woring with NamedGimmickLocatorSets.
        /// </summary>
        public static class Named
        {
            /// <summary>
            /// Create a NamedGimmickLocator with random values.
            /// </summary>
            /// <param name="randomFloat">Function to generate a random float.</param>
            /// <returns>The generated locator.</returns>
            private static NamedGimmickLocator CreateLocator(RandomFloat randomFloat)
            {
                var position = new Vector4(randomFloat(), randomFloat(), randomFloat(), randomFloat());
                var rotation = new Quaternion(randomFloat(), randomFloat(), randomFloat(), randomFloat());
                var locatorName = 424242u;
                var dataSetName = 191919u;
                var locator = new NamedGimmickLocator(position, rotation, locatorName, dataSetName);
                return locator;
            }

            /// <summary>
            /// Create a NamedGimmickLocatorSet with random values.
            /// </summary>
            /// <param name="locatorCount">Number of locators to generate.</param>
            /// <returns>The generated locatorSet.</returns>
            public static GimmickLocatorSet CreateRandomLocatorSet(int locatorCount)
            {
                var random = new Random();
                Func<NamedGimmickLocator> createLocator = () => CreateLocator(() => GetRandomFloat(random));

                var locators = new List<NamedGimmickLocator>();
                for (int i = 0; i < locatorCount; i++)
                {
                    locators.Add(createLocator());
                }

                var locatorSet = GimmickLocatorSet.NewNamedGimmickLocatorSet(locators);
                return locatorSet;
            }
        }

        /// <summary>
        /// Examples of woring with ScaledGimmickLocatorSets.
        /// </summary>
        public static class Scaled
        {
            /// <summary>
            /// Create a ScaledGimmickLocator with random values.
            /// </summary>
            /// <param name="randomFloat">Function to generate a random float.</param>
            /// <returns>The generated locator.</returns>
            private static ScaledGimmickLocator CreateLocator(RandomFloat randomFloat)
            {
                var position = new Vector4(randomFloat(), randomFloat(), randomFloat(), randomFloat());
                var rotation = new Quaternion(randomFloat(), randomFloat(), randomFloat(), randomFloat());
                var scale = new WideVector3(randomFloat(), randomFloat(), randomFloat(), 42, 19);
                var locatorName = 424242u;
                var dataSetName = 191919u;
                var locator = new ScaledGimmickLocator(position, rotation, scale, locatorName, dataSetName);
                return locator;
            }

            /// <summary>
            /// Create a ScaledGimmickLocatorSet with random values.
            /// </summary>
            /// <param name="locatorCount">Number of locators to generate.</param>
            /// <returns>The generated locatorSet.</returns>
            public static GimmickLocatorSet CreateRandomLocatorSet(int locatorCount)
            {
                var random = new Random();
                Func<ScaledGimmickLocator> createLocator = () => CreateLocator(() => GetRandomFloat(random));

                var locators = new List<ScaledGimmickLocator>();
                for (int i = 0; i < locatorCount; i++)
                {
                    locators.Add(createLocator());
                }

                var locatorSet = GimmickLocatorSet.NewScaledGimmickLocatorSet(locators);
                return locatorSet;
            }
        }

        /// <summary>
        /// Writes a GimmickLocatorSet to a .lba file.
        /// </summary>
        /// <param name="locatorSet">LocatorSet to write.</param>
        /// <param name="outputPath">Path of the file to write.</param>
        public static void WriteLocatorSet(GimmickLocatorSet locatorSet, string outputPath)
        {
            using (var writer = new BinaryWriter(new FileStream(outputPath, FileMode.Create)))
            {
                Action<int> writeEmptyBytes = numberOfBytes => WriteEmptyBytes(writer, numberOfBytes);
                var writeFunctions = new FoxLib.Tpp.GimmickLocatorSet.WriteFunctions(
                    writer.Write, writer.Write, writer.Write, writer.Write, writeEmptyBytes);
                FoxLib.Tpp.GimmickLocatorSet.Write(locatorSet, writeFunctions);
            }
        }

        /// <summary>
        /// Reads a .lba file and parses it into a GimmickLocatorSet.
        /// </summary>
        /// <param name="inputPath">File to read.</param>
        /// <returns>The parsed locatorSet.</returns>
        public static GimmickLocatorSet ReadLocatorSet(string inputPath)
        {
            using (var reader = new BinaryReader(new FileStream(inputPath, FileMode.Open)))
            {
                Action<int> skipBytes = numberOfBytes => SkipBytes(reader, numberOfBytes);
                var readFunctions = new FoxLib.Tpp.GimmickLocatorSet.ReadFunctions(
                    reader.ReadSingle, reader.ReadUInt16, reader.ReadUInt32, reader.ReadInt32, skipBytes);
                return FoxLib.Tpp.GimmickLocatorSet.Read(readFunctions);
            }
        }

        /// <summary>
        /// Generates a random float.
        /// </summary>
        /// <param name="random">Random instance.</param>
        /// <returns>The generated float.</returns>
        private static float GetRandomFloat(Random random)
        {
            return (float)random.NextDouble();
        }

        /// <summary>
        /// Writes a number of empty bytes.
        /// </summary>
        /// <param name="writer">The BinaryWriter to use.</param>
        /// <param name="numberOfBytes">The number of empty bytes to write.</param>
        private static void WriteEmptyBytes(BinaryWriter writer, int numberOfBytes)
        {
            writer.Write(new byte[numberOfBytes]);
        }

        /// <summary>
        /// Skip reading a number of bytes.
        /// </summary>
        /// <param name="reader">The BinaryReader to use.</param>
        /// <param name="numberOfBytes">The number of bytes to skip.</param>
        private static void SkipBytes(BinaryReader reader, int numberOfBytes)
        {
            reader.BaseStream.Position += numberOfBytes;
        }
    }
}
