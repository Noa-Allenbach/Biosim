using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biosim.Models
{
    public class Gene
    {
        //public enum NodeType
        //{
        //    Sensor,
        //    Neuron,
        //    Action
        //}

        // Connection properties
        public Node.NodeType SourceType { get; set; }
        public ushort SourceNum { get; set; }
        public Node.NodeType SinkType { get; set; }
        public ushort SinkNum { get; set; }
        public float Weight { get; set; }

        private static Random random = new Random();

        // Static method to create a random gene
        public static Gene MakeRandomGene()
        {
            return new Gene
            {
                SourceType = (Node.NodeType)(random.Next(2)),  // Randomly Sensor (0) or Neuron (1)
                SourceNum = (ushort)random.Next(0, 32767),
                SinkType = (Node.NodeType)(random.Next(2)),  // Randomly Sensor (0) or Neuron (1)
                SinkNum = (ushort)random.Next(0, 32767),
                Weight = MakeRandomWeight()
            };
        }

        // Helper method to generate a random weight
        public static float MakeRandomWeight()
        {
            // Generates a float between -1.0 and 1.0
            return (float)(random.NextDouble() * 2.0 - 1.0);
        }

        public override string ToString()
        {
            return $"Gene(Source: {SourceType} {SourceNum}, Sink: {SinkType} {SinkNum}, Weight: {Weight:F3})";
        }
    }
}
