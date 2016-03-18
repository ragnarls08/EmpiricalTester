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
            // When a new arc (v, w) has v>w, do the search by calling VERTEX-GUIDEDSEARCH(v,w)
            if (nodeOrder.query(nodes[v], nodes[w]))
                return vertexGuidedSearch(v, w);

            return true; // TODO not correct
        }

        public void addVertex()
        {
            //nodes.Add(new DataStructures.SGTNode<HKMSTNode>(new HKMSTNode()));
            if (nodeOrder.Root == null)
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


        private bool vertexGuidedSearch(int iv, int iw)
        {
            var v = nodes[iv];
            var w = nodes[iv];

            // Article states double linked list for F,B and normal list for FL BL
            var F = new List<DataStructures.SGTNode<HKMSTNode>>();
            var B = new List<DataStructures.SGTNode<HKMSTNode>>();
            F.Add(w);
            B.Add(v);

            w.Value.OutEnum = w.Value.outgoing.GetEnumerator();
            w.Value.OutEnum.MoveNext();
            v.Value.InEnum = v.Value.incoming.GetEnumerator();
            v.Value.InEnum.MoveNext();

            var FL = new SortedSet<DataStructures.SGTNode<HKMSTNode>>();
            var BL = new SortedSet<DataStructures.SGTNode<HKMSTNode>>();

            if (w.Value.OutEnum.Current != null)
                FL.Add(w);
            if (v.Value.InEnum.Current != null)
                BL.Add(v);
            
            while(minLessThanMax(FL, BL))
            {
                var u = FL.Min();
                var z = BL.Max();
                // SEARCH-STEP(vertex u, vertex z)

                // We denote by first-out(x) and first-in(x) the first arc on the outgoing 
                // list and the first arc on the incoming list of vertex x, respectively.
                // We denote by next-out((x, y)) and next-in((x, y)) the arcs after (x, y) 
                // on the outgoing list of x and the incoming list of y, respectively. 
                // In each case, if there is no such arc, the value is null.

                // For each vertex x in F, we maintain a forward pointer out(x)to the first 
                // untraversed arc on its outgoing list
                // (u,x) = out(u) = next-out((u,x))
                
                var x = u.Value.OutEnum.Current;
                var y = z.Value.InEnum.Current;
                u.Value.OutEnum.MoveNext();
                z.Value.InEnum.MoveNext();

                if (u.Value.OutEnum.Current == null)
                    FL.Remove(u); 
                if (z.Value.InEnum.Current == null)
                    BL.Remove(z); 

                if (B.Contains(x))
                    return false; // Pair(uz.from, x.Current);
                else if (F.Contains(y))
                    return false; // Pair(y.Current, uz.to);
                

                if(!F.Contains(x))
                {
                    F.Add(x);
                    x.Value.OutEnum = x.Value.outgoing.GetEnumerator();
                    x.Value.OutEnum.MoveNext();
                    if (x.Value.OutEnum.Current != null)
                        FL.Add(x);
                }
                if(!B.Contains(y))
                {
                    B.Add(y);
                    y.Value.InEnum = y.Value.incoming.GetEnumerator();
                    y.Value.InEnum.MoveNext();
                    if (y.Value.InEnum.Current != null)
                        BL.Add(y);
                }              
                // End of SEARCH-STEP(vertex u, vertex z)
            }
            
            return true;
        }

        private bool minLessThanMax(SortedSet<DataStructures.SGTNode<HKMSTNode>> FL,
                                    SortedSet<DataStructures.SGTNode<HKMSTNode>> BL)
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

        private class Pair
        {
            public DataStructures.SGTNode<HKMSTNode> from { get; set; }
            public DataStructures.SGTNode<HKMSTNode> to { get; set; }

            public Pair(DataStructures.SGTNode<HKMSTNode> from, DataStructures.SGTNode<HKMSTNode> to)
            {
                this.from = from;
                this.to = to;
            }
        }

    }
}
