using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biosim.Models
{
    public class Node
    {
        public enum NodeType
        {
            Sensor = 0,
            Neuron = 1,
            Action = 2, // Add Action as a type
        }

        public NodeType Type { get; set; }
        public ushort Number { get; set; } // Original number
        public ushort RemappedNumber { get; set; } // Updated/Sequential number for efficient processing
                                                   // Add the properties to track inputs and outputs
        public ushort Id { get; set; }
        public int NumSelfInputs { get; set; }
        public int NumInputsFromSensorsOrOtherNeurons { get; set; }
        public int NumOutputs { get; set; }

        // Constructor to initialize with type and id
        public Node(NodeType type, ushort id, ushort initialInputs = 0)
        {
            Type = type;
            Id = id;
            NumSelfInputs = initialInputs;
            NumInputsFromSensorsOrOtherNeurons = 0;
            NumOutputs = 0;
        }
        public override string ToString()
        {
            return $"Node(Type: {Type}, Number: {Number}, Remapped: {RemappedNumber})";
        }
    }
}
