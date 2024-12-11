using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Biosim.Models
{
    public class ConnectionList : List<Gene>
    {
        // Additional methods specific to ConnectionList can be added here if needed
        public static void UpdateConnectionList(List<Gene> connectionList, Dictionary<ushort, Node> nodeMap)
        {
            var updatedConnections = new List<Gene>();

            foreach (var conn in connectionList)
            {
                Gene updatedConn = conn;

                // Check the source type
                if (updatedConn.SourceType == Gene.NodeType.Neuron && nodeMap.ContainsKey(updatedConn.SourceNum))
                {
                    updatedConn.SourceNum = nodeMap[updatedConn.SourceNum].RemappedNumber;
                }

                // Check the sink type
                if (updatedConn.SinkType == Gene.NodeType.Neuron && nodeMap.ContainsKey(updatedConn.SinkNum))
                {
                    updatedConn.SinkNum = nodeMap[updatedConn.SinkNum].RemappedNumber;
                }

                updatedConnections.Add(updatedConn);
            }

            connectionList.Clear();
            connectionList.AddRange(updatedConnections);
        }
    }

 }

