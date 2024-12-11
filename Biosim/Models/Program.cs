using System;
using System.Collections.Generic;

namespace Biosim.Models
{
    class Program
    {
        static void Main(string[] args)
        {
            // Step 1: Create a test genome with simple nodes and connections
            Dictionary<ushort, Node> nodeMap = new Dictionary<ushort, Node>();
            nodeMap.Add(0, new Node(Node.NodeType.Sensor, 0)); // Sensor 0
            nodeMap.Add(1, new Node(Node.NodeType.Neuron, 1)); // Neuron 1
            nodeMap.Add(2, new Node(Node.NodeType.Action, 2)); // Action 2

            // Step 2: Create some genes to connect the nodes
            List<Gene> connectionList = new List<Gene>
            {
                new Gene { SourceType = Gene.NodeType.Sensor, SourceNum = 0, SinkType = Gene.NodeType.Neuron, SinkNum = 1, Weight = 0.5f },
                new Gene { SourceType = Gene.NodeType.Neuron, SourceNum = 1, SinkType = Gene.NodeType.Action, SinkNum = 2, Weight = 1.2f }
            };

            // Step 3: Create a NeuralNetworkProcessor
            NeuralNetworkProcessor processor = new NeuralNetworkProcessor(nodeMap, connectionList);

            // Step 4: Set inputs for sensor nodes (e.g., Sensor 0 = 1.0)
            processor.SetInputs(new Dictionary<ushort, float> { { 0, 1.0f } });

            // Step 5: Propagate the inputs through the network
            processor.Propagate();

            // Step 6: Get the output values from the action nodes (in this case, Action 2)
            var outputs = processor.GetOutputs(new List<ushort> { 2 });

            // Step 7: Print the output
            Console.WriteLine("Output values:");
            foreach (var output in outputs)
            {
                Console.WriteLine($"Node {output.Key}: {output.Value}");
            }

            // Optional: You can further add assertions or checks here to validate the outputs.
        }
    }
}
