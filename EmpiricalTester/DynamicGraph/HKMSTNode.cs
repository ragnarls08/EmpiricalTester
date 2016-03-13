using System.Collections.Generic;

namespace EmpiricalTester.DynamicGraph
{
    internal class HKMSTNode 
    {
        public List<int> incoming{ get; set; }
        public List<int> outgoing { get; set; }

        public HKMSTNode()
        {
            incoming = new List<int>();
            outgoing = new List<int>();
        }
    }
}