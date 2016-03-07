using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpiricalTester.DynamicGraph
{
    class SimpleNode
    {
        public int level { get; set; }
        public bool blackHole { get; set; }
        public bool visited { get; set; }

        public List<int> incoming;

        public SimpleNode()
        {
            incoming = new List<int>();
            blackHole = false;
            visited = false;
        }
    }
}
