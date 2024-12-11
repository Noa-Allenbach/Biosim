using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Biosim.Models.Gene;
using static Biosim.Models.Node;

namespace Biosim.Models
{
    public class Genome : List<Gene> { }

    static class GenomeFunctions
    {
        private static Random random = new Random();

        public static Gene MakeRandomGene()
        {
            var gene = new Gene
            {
                // Cast the byte to the NodeType enum
                SourceType = (Node.NodeType)(random.Next(2)),  // Randomly Sensor (0) or Neuron (1)
                SourceNum = (ushort)random.Next(0, 0x7FFF),
                SinkType = (Node.NodeType)(random.Next(2)),  // Randomly Sensor (0) or Neuron (1)
                SinkNum = (ushort)random.Next(0, 0x7FFF),
                Weight = Gene.MakeRandomWeight()
            };
            return gene;
        }

        public static Genome MakeRandomGenome()
        {
            var genome = new Genome();
            int length = random.Next(Parameters.GenomeInitialLengthMin, Parameters.GenomeInitialLengthMax);
            for (int n = 0; n < length; ++n)
            {
                genome.Add(MakeRandomGene());
            }
            return genome;
        }

        public static void MakeRenumberedConnectionList(List<Gene> connectionList, Genome genome)
        {
            connectionList.Clear();

            foreach (var gene in genome)
            {
                // Copy gene to avoid modifying the original
                var conn = new Gene
                {
                    SourceType = gene.SourceType,
                    SourceNum = gene.SourceNum,
                    SinkType = gene.SinkType,
                    SinkNum = gene.SinkNum,
                    Weight = gene.Weight
                };

                // Apply modulo to source and sink numbers based on their types
                if (conn.SourceType == Node.NodeType.Neuron)
                {
                    conn.SourceNum %= Parameters.MaxNumberNeurons;
                }
                else
                {
                    // Apply modulo for sensors to ensure they are within the valid range
                    conn.SourceNum %= Parameters.MaxNumberSensors;
                }

                if (conn.SinkType == Node.NodeType.Neuron)
                {
                    // Apply modulo for neurons to ensure they are within the valid range
                    conn.SinkNum %= Parameters.MaxNumberNeurons;
                }
                else
                {
                    // Apply modulo for sensors to ensure they are within the valid range
                    conn.SinkNum %= Parameters.MaxNumberSensors;
                }

                connectionList.Add(conn);
            }
        }

        public static void MakeNodeList(Dictionary<ushort, Node> nodeMap, List<Gene> connectionList)
        {
                nodeMap.Clear();

            foreach (var conn in connectionList)
            {
                // Process the sink (target) node of the connection
                if (conn.SinkType == NodeType.Neuron || conn.SinkType == NodeType.Action)
                {
                    if (!nodeMap.TryGetValue(conn.SinkNum, out var sinkNode))
                    {
                        sinkNode = new Node((Node.NodeType)conn.SinkType, conn.SinkNum, 0);
                        // Include type and id for actions
                        nodeMap[conn.SinkNum] = sinkNode;
                    }

                    if (conn.SourceType == NodeType.Neuron && conn.SourceNum == conn.SinkNum)
                    {
                        sinkNode.NumSelfInputs++;
                    }
                    else
                    {
                        sinkNode.NumInputsFromSensorsOrOtherNeurons++;
                    }
                }

                // Process the source node of the connection
                if (conn.SourceType == NodeType.Neuron)
                {
                    if (!nodeMap.TryGetValue(conn.SourceNum, out var sourceNode))
                    {
                        new Node((Node.NodeType)conn.SourceType, conn.SourceNum, 0);

                        nodeMap[conn.SourceNum] = sourceNode;
                    }

                    sourceNode.NumOutputs++;
                }
            }
        }

        public static void RemoveConnectionsToNeuron(List<Gene> connections, Dictionary<ushort, Node> nodeMap, ushort neuronNumber)
        {
                for (int i = connections.Count - 1; i >= 0; i--)
                {
                    var connection = connections[i];

                    // Check if the connection’s sink is the target neuron
                    if (connection.SinkType == NodeType.Neuron && connection.SinkNum == neuronNumber)
                    {
                        // If the source is a neuron, reduce its output count in the NodeMap
                        if (connection.SourceType == NodeType.Neuron && nodeMap.TryGetValue(connection.SourceNum, out Node sourceNode))
                        {
                            sourceNode.NumOutputs--;
                        }

                        // Remove the connection to the target neuron
                        connections.RemoveAt(i);
                    }
                }
            }

        public static void CullUselessNeurons(List<Gene> connections, Dictionary<ushort, Node> nodeMap)
        {
            bool allDone = false;

            while (!allDone)
            {
                allDone = true;

                // Iterate over the nodeMap to find useless neurons
                foreach (var neuron in nodeMap.ToList()) // Create a copy of nodeMap to safely modify it
                {
                    ushort neuronId = neuron.Key;
                    Node node = neuron.Value;

                    // Check if the neuron has no outputs or only feeds itself
                    if (node.NumOutputs == node.NumSelfInputs)
                    {
                        allDone = false;

                        // Remove all connections to this neuron
                        RemoveConnectionsToNeuron(connections, nodeMap, neuronId);

                        // Remove the neuron from nodeMap
                        nodeMap.Remove(neuronId);
                    }
                }
            }
        }

        public static void RandomBitFlip(Genome genome)
        {
            int size = sizeof(byte) + sizeof(ushort) + sizeof(ushort) + sizeof(float);

            int byteIndex = random.Next(0, genome.Count) * size;
            int elementIndex = random.Next(0, genome.Count);
            byte bitIndex8 = (byte)(1 << random.Next(0, 7));
            float chance = (float)random.NextDouble();

            if (chance < 0.2f)
            {
                // Correct way to apply XOR to an enum type
                genome[elementIndex].SourceType ^= (NodeType)(1 << random.Next(0, 7));

            }
            else if (chance < 0.4f)
            {
                genome[elementIndex].SinkType = (NodeType)((int)genome[elementIndex].SinkType ^ (1 << random.Next(0, 7)));

            }
            else if (chance < 0.6f)
            {
                genome[elementIndex].SourceNum ^= bitIndex8;
            }
            else if (chance < 0.8f)
            {
                genome[elementIndex].SinkNum ^= bitIndex8;
            }
            else
            {
                genome[elementIndex].Weight *= (1 + (float)(random.NextDouble() * 0.1f - 0.05f)); // Random scaling of the weight

            }
        }

        public static void CropLength(Genome genome, int length)
        {
            if (genome.Count > length && length > 0)
            {
                if (random.NextDouble() < 0.5)
                {
                    genome.RemoveRange(0, genome.Count - length);
                }
                else
                {
                    genome.RemoveRange(length, genome.Count - length);
                }
            }
        }

        public static void RandomInsertDeletion(Genome genome)
        {
            if (random.NextDouble() < Parameters.GeneInsertionDeletionRate)
            {
                if (random.NextDouble() < Parameters.DeletionRatio)
                {
                    if (genome.Count > 1)
                    {
                        genome.RemoveAt(random.Next(0, genome.Count));
                    }
                }
                else if (genome.Count < Parameters.GenomeMaxLength)
                {
                    genome.Add(MakeRandomGene());
                }
            }
        }

        public static void ApplyPointMutations(Genome genome)
        {
            foreach (var gene in genome)
            {
                if (random.NextDouble() < Parameters.PointMutationRate)
                {
                    RandomBitFlip(genome);
                }
            }
        }

        public static Genome GenerateChildGenome(List<Genome> parentGenomes)
        {
            Genome genome;
            int parent1Idx = random.Next(parentGenomes.Count);
            int parent2Idx = random.Next(parentGenomes.Count);

            var g1 = parentGenomes[parent1Idx];
            var g2 = parentGenomes[parent2Idx];
            genome = (g1.Count > g2.Count) ? g1 : g2;

            ApplyPointMutations(genome);
            Debug.Assert(genome.Count <= Parameters.GenomeMaxLength);

            return genome;
        }
    }
}
