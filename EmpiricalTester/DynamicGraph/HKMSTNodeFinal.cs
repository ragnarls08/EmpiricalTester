using System.Collections.Generic;

namespace EmpiricalTester.DynamicGraph
{
    internal class HKMSTNodeFinal
    {
        public List<DataStructures.SGTNode<HKMSTNodeFinal>> Incoming { get; set; }
        public List<DataStructures.SGTNode<HKMSTNodeFinal>> Outgoing { get; set; }

        public IEnumerator<DataStructures.SGTNode<HKMSTNodeFinal>> OutEnum { get; set; }
        public IEnumerator<DataStructures.SGTNode<HKMSTNodeFinal>> InEnum { get; set; }
                
        public bool InF { get; set; }
        public bool InB { get; set; }

        public int N;

        public HKMSTNodeFinal()
        {
            Incoming = new List<DataStructures.SGTNode<HKMSTNodeFinal>>();
            Outgoing = new List<DataStructures.SGTNode<HKMSTNodeFinal>>();
            InF = false;
            InB = false;
        }

        public HKMSTNodeFinal(int n)
        {
            Incoming = new List<DataStructures.SGTNode<HKMSTNodeFinal>>();
            Outgoing = new List<DataStructures.SGTNode<HKMSTNodeFinal>>();
            InF = false;
            InB = false;
            N = n;
        }
    }
}
