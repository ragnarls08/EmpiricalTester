using System;
using System.Collections.Generic;
using System.Linq;

namespace EmpiricalTester.DynamicGraph
{
    class HKMST_V1 : IDynamicGraph
    {        
        private DataStructures.SGTree<HKMSTNode> nodeOrder;
        private List<DataStructures.SGTNode<HKMSTNode>> nodes;

        public HKMST_V1(double alpha)
        {
            nodeOrder = new DataStructures.SGTree<HKMSTNode>(alpha);
            nodes = new List<DataStructures.SGTNode<HKMSTNode>>();
        }

        public bool addEdge(int v, int w)
        {
            throw new NotImplementedException();
        }

        public void addVertex()
        {
            // TODO this is not ok since all vertexes will be < root wich will cause a problem if for
            // example the first edge added is (x, root)
            // Possible solution: add all vertexes to a list of objects with the attribute sgtRef = null
            // if the reference is null  x (<|>) root is false since they are not ordered 
            // alternatively add an insert function to SGT insert(sgtnode, sgtnode), probably the best way


            if(nodeOrder.Root == null)
                nodes.Add(nodeOrder.insertFirst(new HKMSTNode()));
            else
                nodes.Add(nodeOrder.insert(nodeOrder.Root, new HKMSTNode()));
        }

        public void removeEdge(int v, int w)
        {
            //graph[w].incoming.Remove(v);
            //graph[w].outgoing.Remove(v);
        }

        public void resetAll()
        {
            //this.graph.Clear();
        }

        public List<int> topology()
        {
            throw new NotImplementedException();
        }


        private void vertexGuidedSearch(int iv, int iw)
        {
            DataStructures.SGTNode<HKMSTNode> v = nodes[iv];
            DataStructures.SGTNode<HKMSTNode> w = nodes[iv];

            // Article states double linked list for F,B and normal list for FL BL
            LinkedList<DataStructures.SGTNode<HKMSTNode>> F = new LinkedList<DataStructures.SGTNode<HKMSTNode>>();
            LinkedList<DataStructures.SGTNode<HKMSTNode>> B = new LinkedList<DataStructures.SGTNode<HKMSTNode>>();
            F.AddFirst(w);
            B.AddFirst(v);

            List<DataStructures.SGTNode<HKMSTNode>> FL = new List<DataStructures.SGTNode<HKMSTNode>>();
            List<DataStructures.SGTNode<HKMSTNode>> BL = new List<DataStructures.SGTNode<HKMSTNode>>();
            if (w.Value.outgoing.Count > 0)
                FL.Add(w);
            if (v.Value.incoming.Count > 0)
                BL.Add(v);

            while(minLessThanMax(FL, BL))
            {                
                var uz = findUZ(FL, BL);
                var u = uz.Item1;
                var z = uz.Item2;
                // SEARCH-STEP(vertex u, vertex z)

                // End of SEARCH-STEP(vertex u, vertex z)
            }

            //return null;
        }

        private Tuple<DataStructures.SGTNode<HKMSTNode>, DataStructures.SGTNode<HKMSTNode>> findUZ(
                                    List<DataStructures.SGTNode<HKMSTNode>> FL,
                                    List<DataStructures.SGTNode<HKMSTNode>> BL)
        {
            // find (u, z) with u < z
            return null;
        }

        private bool minLessThanMax(List<DataStructures.SGTNode<HKMSTNode>> FL, 
                                    List<DataStructures.SGTNode<HKMSTNode>> BL)
        {
            // For ease of notation, we adopt the convention that the
            // minimum of an empty set is bigger than any other value and the maximum of an empty
            // set is smaller than any other value.
            if (FL.Count == 0)
                return false;
            if (BL.Count == 0)
                return false;

            // TODO double check if this is correct
            return FL.Min(item => item.label) < BL.Max(item => item.label); 
        }
    }
}
