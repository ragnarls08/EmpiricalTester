using System.Collections.Generic;

namespace EmpiricalTester.DynamicGraph
{
    internal class HKMSTNode
    {
        public List<DataStructures.SGTNode<HKMSTNode>> Incoming{ get; set; }
        public List<DataStructures.SGTNode<HKMSTNode>> Outgoing { get; set; }
                
        public IEnumerator<DataStructures.SGTNode<HKMSTNode>> OutEnum { get; set; }
        public IEnumerator<DataStructures.SGTNode<HKMSTNode>> InEnum { get; set; }

        public bool InF { get; set; }
        public bool InB { get; set; }

        public int N;

        public HKMSTNode()
        {
            Incoming = new List<DataStructures.SGTNode<HKMSTNode>> ();
            Outgoing = new List<DataStructures.SGTNode<HKMSTNode>> ();
            InF = false;
            InB = false;
        }

        public HKMSTNode(int n)
        {
            Incoming = new List<DataStructures.SGTNode<HKMSTNode>>();
            Outgoing = new List<DataStructures.SGTNode<HKMSTNode>>();
            InF = false;
            InB = false;
            N = n;
        }
    }
}