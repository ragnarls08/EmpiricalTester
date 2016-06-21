using System.Collections.Generic;

namespace EmpiricalTester.DynamicGraph
{
    internal class BFGTNode
    {
        public int Level { get; set; }
        public int Index { get; set; }        
        public int Label { get; set; }
        public bool Visited { get; set; }
        public int BK => _b * Level;

        public List<int> Outgoing { get; set; }
        public List<int> Incoming { get; set; }

        private int _b;

        public BFGTNode(int level, int index, int label, int b)
        {
            Level = level;
            Index = index;
            Label = label;
            _b = b;
            Visited = false;

            Outgoing = new List<int>();
            Incoming = new List<int>();
        }

        public BFGTNode(int level, int index, int label, int b, List<int> incoming, List<int> outgoing)
        {
            Level = level;
            Index = index;
            Label = label;
            _b = b;
            Visited = false;

            Outgoing = outgoing;
            Incoming = incoming;
        }

    }
}
