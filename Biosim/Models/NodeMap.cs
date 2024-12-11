using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biosim.Models
{
    public class NodeMap : Dictionary<ushort, Node>
    {
        // Additional methods specific to NodeMap can be added here if needed
        // Renumber neurons in the nodeMap sequentially starting at 0
        private static void RenumberNeurons(Dictionary<ushort, Node> nodeMap)
        {
            ushort newNumber = 0;

            foreach (var neuron in nodeMap.OrderBy(n => n.Key)) // Ensure consistent ordering
            {
                Node node = neuron.Value;

                // Assign a new sequential number
                node.RemappedNumber = newNumber++;

                // Update the node in the dictionary
                nodeMap[neuron.Key] = node;
            }
        }
    }
}
