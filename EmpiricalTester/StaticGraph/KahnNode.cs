using System.Collections.Generic;

namespace EmpiricalTester.StaticGraph
{
    internal class KahnNode
    {
        public List<int> Incoming { get; set; }
        public List<int> Outgoing { get; set; }

        public KahnNode()
        {
            Incoming = new List<int>();
            Outgoing = new List<int>();
        }

        // copy constructor
        public KahnNode(KahnNode node)
        {
            Incoming = new List<int>(node.Incoming);
            Outgoing = new List<int>(node.Outgoing);
        }
    }
}
