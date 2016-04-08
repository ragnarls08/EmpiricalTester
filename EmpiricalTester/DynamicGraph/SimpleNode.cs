using System.Collections.Generic;

namespace EmpiricalTester.DynamicGraph
{
    internal class SimpleNode
    {
        public int Level { get; set; }
        public bool BlackHole { get; set; }

        public List<int> Incoming;

        public SimpleNode()
        {
            Incoming = new List<int>();
            BlackHole = false;
        }
    }
}
