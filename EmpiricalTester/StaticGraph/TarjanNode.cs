using System.Collections.Generic;

namespace EmpiricalTester.StaticGraph
{
    class TarjanNode
    {
        public List<int> Incoming { get; set; }
        public List<int> Outgoing { get; set; }
        public bool Visited { get; set; }
        public bool TemporaryVisit { get; set; }

        public TarjanNode()
        {
            Incoming = new List<int>();
            Outgoing = new List<int>();
            Visited = false;
            TemporaryVisit = false;
        }
    }
}
