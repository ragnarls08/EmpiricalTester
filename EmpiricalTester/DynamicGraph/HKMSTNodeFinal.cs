using System.Collections.Generic;

namespace EmpiricalTester.DynamicGraph
{
    internal class HKMSTNodeFinal
    {
        public List<DataStructures.SGTNode<HKMSTNodeFinal>> incoming { get; set; }
        public List<DataStructures.SGTNode<HKMSTNodeFinal>> outgoing { get; set; }

        public IEnumerator<DataStructures.SGTNode<HKMSTNodeFinal>> OutEnum { get; set; }
        public IEnumerator<DataStructures.SGTNode<HKMSTNodeFinal>> InEnum { get; set; }
                
        public bool Active { get; set; }
        public bool InF { get; set; }
        public bool InB { get; set; }

        public int n;

        public HKMSTNodeFinal()
        {
            incoming = new List<DataStructures.SGTNode<HKMSTNodeFinal>>();
            outgoing = new List<DataStructures.SGTNode<HKMSTNodeFinal>>();
            this.Active = false;
            this.InF = false;
            this.InB = false;
        }

        public HKMSTNodeFinal(int n)
        {
            incoming = new List<DataStructures.SGTNode<HKMSTNodeFinal>>();
            outgoing = new List<DataStructures.SGTNode<HKMSTNodeFinal>>();
            this.Active = false;
            this.InF = false;
            this.InB = false;
            this.n = n;
        }
    }
}
