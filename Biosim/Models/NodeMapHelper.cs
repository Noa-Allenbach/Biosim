using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biosim.Models
{
    public static class NodeMapHelper
    {
        public static Dictionary<ushort, Node> InitializeNodeMap(List<Gene> genes)
        {
            var nodeMap = new Dictionary<ushort, Node>();
            ushort remappedCounter = 0; // Keeps track of the new IDs

            foreach (var gene in genes)
            {
                // Process source neurons
                if (gene.SourceType == Gene.NodeType.Neuron && !nodeMap.ContainsKey(gene.SourceNum))
                {
                    nodeMap[gene.SourceNum] = new Node(Node.NodeType.Neuron, gene.SourceNum, remappedCounter++);
                    //{
                    //    OriginalNumber = gene.SourceNum,
                    //    RemappedNumber = remappedCounter++
                    //};
                }

                // Process sink neurons
                if (gene.SinkType == Gene.NodeType.Neuron && !nodeMap.ContainsKey(gene.SinkNum))
                {
                    nodeMap[gene.SinkNum] = new Node(Node.NodeType.Neuron, gene.SourceNum, remappedCounter++);
                    //{
                    //    OriginalNumber = gene.SinkNum,
                    //    RemappedNumber = remappedCounter++
                    //};
                }
            }

            return nodeMap;
        }
    }
}
