using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biosim.Models
{
    public class NeuralNetworkProcessor
    {
        private Dictionary<ushort, float> nodeValues;
        private List<Gene> connections;
        private Dictionary<ushort, Node> nodeMap;

        public NeuralNetworkProcessor()
        {
            nodeMap = new Dictionary<ushort, Node>();
            connections = new List<Gene>();
            nodeValues = new Dictionary<ushort, float>();
        }

        public NeuralNetworkProcessor(Dictionary<ushort, Node> nodeMap, List<Gene> connectionList)
        {
            // Initialize node values and connections
            nodeValues = new Dictionary<ushort, float>();
            foreach (var node in nodeMap.Values)
            {
                nodeValues[node.RemappedNumber] = 0.0f; // Initial value for all nodes
            }

            connections = connectionList;
        }

        public void ApplyGenome(Genome genome)
        {
            nodeMap.Clear();
            connections.Clear();

            // Build nodes from genome
            foreach (var gene in genome)
            {
                if (!nodeMap.ContainsKey(gene.SourceNum))
                {
                    nodeMap[gene.SourceNum] = new Node(gene.SourceType, gene.SourceNum);
                }

                if (!nodeMap.ContainsKey(gene.SinkNum))
                {
                    nodeMap[gene.SinkNum] = new Node(gene.SinkType, gene.SinkNum);
                }

                connections.Add(gene);
            }
        }

        public void InitializeNodeValues(Dictionary<ushort, float> initialValues)
        {
            nodeValues.Clear();
            foreach (var kvp in initialValues)
            {
                nodeValues[kvp.Key] = kvp.Value;
            }
        }

        /// <summary>
        /// Sets input values to the sensor nodes.
        /// </summary>
        public void SetInputs(Dictionary<ushort, float> inputs)
        {
            foreach (var input in inputs)
            {
                if (nodeValues.ContainsKey(input.Key))
                {
                    nodeValues[input.Key] = input.Value;
                }
                else
                {
                    throw new ArgumentException($"Input node {input.Key} does not exist in the network.");
                }
            }
        }

        /// <summary>
        /// Propagates values through the network.
        /// </summary>
        public void Propagate()
        {
            var newValues = new Dictionary<ushort, float>(nodeValues);

            foreach (var gene in connections)
            {
                if (nodeValues.ContainsKey(gene.SourceNum))
                {
                    float sourceValue = nodeValues[gene.SourceNum];
                    float weightedValue = sourceValue * gene.Weight;

                    if (newValues.ContainsKey(gene.SinkNum))
                    {
                        newValues[gene.SinkNum] += weightedValue;
                    }
                    else
                    {
                        newValues[gene.SinkNum] = weightedValue;
                    }
                }
            }

            nodeValues = newValues;
        }

        public Dictionary<ushort, float> GetOutputs(List<ushort> outputNodes)
        {
            var outputs = new Dictionary<ushort, float>();

            foreach (var node in outputNodes)
            {
                if (nodeValues.ContainsKey(node))
                {
                    outputs[node] = nodeValues[node];
                }
                else
                {
                    throw new ArgumentException($"Output node {node} does not exist in the network.");
                }
            }

            return outputs;
        }
    }

}
