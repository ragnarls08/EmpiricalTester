using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpiricalTester.StaticGraph
{
    class TarjanNode
    {
        public List<int> incoming { get; set; }
        public List<int> outgoing { get; set; }
        public bool visited { get; set; }
        public bool temporaryVisit { get; set; }

        public TarjanNode()
        {
            incoming = new List<int>();
            outgoing = new List<int>();
            visited = false;
            temporaryVisit = false;
        }
    }
}
