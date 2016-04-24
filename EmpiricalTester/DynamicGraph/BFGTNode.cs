

using System.Collections.Generic;

namespace EmpiricalTester.DynamicGraph
{
    internal class BFGTNode
    {
        public int Level { get; set; }
        public int Index { get; set; }
        public int Label { get; set; }
        public bool Visited { get; set; }

        public List<BFGTNode> Outgoing { get; set; }
        public List<BFGTNode> Incoming { get; set; }

        public BFGTNode(int level, int index, int label)
        {
            Level = level;
            Index = index;
            Label = label;
            Visited = false;

            Outgoing = new List<BFGTNode>();
            Incoming = new List<BFGTNode>();
        }

    }
}
