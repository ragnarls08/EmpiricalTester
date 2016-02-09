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
    }
}
