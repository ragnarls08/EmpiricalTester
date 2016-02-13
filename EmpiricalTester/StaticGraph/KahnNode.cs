using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpiricalTester.StaticGraph
{
    class KahnNode
    {
        public List<int> incoming { get; set; }
        public List<int> outgoing { get; set; }

        public KahnNode()
        {
            incoming = new List<int>();
            outgoing = new List<int>();
        }

        // copy constructor
        public KahnNode(KahnNode node)
        {
            this.incoming = new List<int>(node.incoming);
            this.outgoing = new List<int>(node.outgoing);
        }
    }
}
