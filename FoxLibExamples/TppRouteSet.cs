using System;
using System.Collections.Generic;
using System.IO;
using static FoxLib.Core;
using static FoxLib.Tpp.RouteSet;

namespace FoxLibExamples
{
    public static class TppRouteSetExamples
    {
        /// <summary>
        /// Create a route node.
        /// </summary>
        /// <returns>The created route node.</returns>
        private static RouteNode CreateRouteNode()
        {
            var position = new Vector3(1.0f, 2.0f, 3.0f);

            var event0 = new RouteEvent(1530489467, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, "abcd");
            var event1 = new RouteEvent(1432398056, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, "efgh");
            var event2 = new RouteEvent(4202868537, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, "ijkl");

            var events = new List<RouteEvent>() { event0, event1, event2 };
            var node = new RouteNode(position, event0, events);

            return node;
        }

        /// <summary>
        /// Create a route.
        /// </summary>
        /// <returns>The created route.</returns>
        private static Route CreateRoute()
        {
            var node0 = CreateRouteNode();
            var node1 = CreateRouteNode();
            var node2 = CreateRouteNode();

            var routeId = 293799188u;
            var nodes = new List<RouteNode>() { node0, node1, node2 };
            return new Route(routeId, nodes);
        }

        /// <summary>
        /// Create a routeset.
        /// </summary>
        /// <returns>The created routeset.</returns>
        public static RouteSet CreateRouteSet()
        {
            var route0 = CreateRoute();
            var route1 = CreateRoute();
            var route2 = CreateRoute();

            var routes = new List<Route>() { route0, route1, route2 };
            var routeset = new RouteSet(routes);
            return routeset;
        }

        /// <summary>
        /// Writes a RouteSet to a .frt file.
        /// </summary>
        /// <param name="routeset">RouteSet to write.</param>
        /// <param name="outputPath">Path of the file to write.</param>
        public static void WriteRouteSet(RouteSet routeset, string outputPath)
        {
            using (var writer = new BinaryWriter(new FileStream(outputPath, FileMode.Create), getEncoding()))
            {
                Action<int> writeEmptyBytes = numberOfBytes => WriteEmptyBytes(writer, numberOfBytes);
                var writeFunctions = new WriteFunctions(writer.Write, writer.Write, writer.Write, writer.Write, writer.Write, writeEmptyBytes);
                Write(writeFunctions, routeset);
            }
        }

        /// <summary>
        /// Reads a .frt file and parses it into a RouteSet.
        /// </summary>
        /// <param name="inputPath">File to read.</param>
        /// <returns>The parsed routeset.</returns>
        public static RouteSet ReadRouteSet(string inputPath)
        {
            using (var reader = new BinaryReader(new FileStream(inputPath, FileMode.Open), getEncoding()))
            {
                Action<int> skipBytes = numberOfBytes => SkipBytes(reader, numberOfBytes);
                var readFunctions = new ReadFunctions(reader.ReadSingle, reader.ReadUInt16, reader.ReadUInt32, reader.ReadInt32, reader.ReadBytes, skipBytes);
                return Read(readFunctions);
            }
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