using System.Collections.Generic;

namespace EmpiricalTester.DynamicGraph
{
    internal class SimpleNode
    {
        public int level { get; set; }
        public bool blackHole { get; set; }

        public List<int> incoming;

        public SimpleNode()
        {
            incoming = new List<int>();
            blackHole = false;
        }
    }
}
