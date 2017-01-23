using System;
using System.Collections.Generic;

namespace EmpiricalTester.DynamicGraph
{
    internal class BFGTNode
    {
        public UInt64 Level { get; set; }
        public UInt64 Index { get; set; }        
        public int Label { get; set; }
        public bool Visited { get; set; }
        public bool VisitedF { get; set; }
        public UInt64 BK => _b * Level;

        public List<int> Outgoing { get; set; }
        public List<int> Incoming { get; set; }

        private UInt64 _b;

        public BFGTNode(UInt64 level, UInt64 index, int label, UInt64 b)
        {
            Level = level;
            Index = index;
            Label = label;
            _b = b;
            Visited = false;
            VisitedF = false;
            
            Outgoing = new List<int>();
            Incoming = new List<int>();
        }

        public BFGTNode(UInt64 level, UInt64 index, int label, UInt64 b, List<int> incoming, List<int> outgoing)
        {
            Level = level;
            Index = index;
            Label = label;
            _b = b;
            Visited = false;
            VisitedF = false;

            Outgoing = outgoing;
            Incoming = incoming;
        }

    }
}
