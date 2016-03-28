using System.Collections.Generic;

namespace EmpiricalTester.DynamicGraph
{
    internal class HKMSTNode
    {
        public List<DataStructures.SGTNode<HKMSTNode>> incoming{ get; set; }
        public List<DataStructures.SGTNode<HKMSTNode>> outgoing { get; set; }
                
        public IEnumerator<DataStructures.SGTNode<HKMSTNode>> OutEnum { get; set; }
        public IEnumerator<DataStructures.SGTNode<HKMSTNode>> InEnum { get; set; }

        public bool InF { get; set; }
        public bool InB { get; set; }

        public int n;

        public HKMSTNode()
        {
            incoming = new List<DataStructures.SGTNode<HKMSTNode>> ();
            outgoing = new List<DataStructures.SGTNode<HKMSTNode>> ();
            this.InF = false;
            this.InB = false;
        }

        public HKMSTNode(int n)
        {
            incoming = new List<DataStructures.SGTNode<HKMSTNode>>();
            outgoing = new List<DataStructures.SGTNode<HKMSTNode>>();
            this.InF = false;
            this.InB = false;
            this.n = n;
        }
    }
}