using System.Collections.Generic;
namespace EmpiricalTester.DynamicGraph
{
    class PKNode
    {
        public int Index { get; set; }
        public int Ord { get; set; }
        public bool Visited { get; set; }
        public List<PKNode> Incoming { get; set; }
        public List<PKNode> Outgoing { get; set; }

        public PKNode(int index, int ord)
        {
            Index = index;
            Ord = ord;
            Visited = false;

            Incoming = new List<PKNode>();
            Outgoing = new List<PKNode>();
        }
    }
}
