using System;
using System.Collections.Generic;
using System.Linq;

namespace EmpiricalTester.DynamicGraph
{
    public class HKMST_V1 : IDynamicGraph
    {        
        private DataStructures.SGTree<HKMSTNode> nodeOrder;
        private List<DataStructures.SGTNode<HKMSTNode>> nodes;

        private int nCount = 0;//debug
        private enum insertDirection { Before, After };

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
            else
            {
                nodes[v].Value.outgoing.Add(nodes[w]);
                nodes[w].Value.incoming.Add(nodes[v]);
            }

            return true; 
        }

        public void addVertex()
        {
            //nodes.Add(new DataStructures.SGTNode<HKMSTNode>(new HKMSTNode()));
            if (nodeOrder.Root == null)
                nodes.Add(nodeOrder.insertFirst(new HKMSTNode(nCount)));
            else
                nodes.Add(nodeOrder.insertAfter(nodeOrder.Root, new HKMSTNode(nCount)));

            nCount++;
        }

        public void removeEdge(int v, int w)
        {
            //graph[w].incoming.Remove(v);
            //graph[w].outgoing.Remove(v);
        }

        public void resetAll()
        {
            this.nCount = 0;
            this.nodeOrder.Clear();
            this.nodes.Clear();
        }

        public List<int> topology()
        {
            return nodeOrder.inOrder().ConvertAll<int>(item => item.n);            
        }


        private bool vertexGuidedSearch(int iv, int iw)
        {
            var v = nodes[iv];
            var w = nodes[iw];

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

            // For ease of notation, we adopt the convention that the
            // minimum of an empty set is bigger than any other value and the maximum of an empty
            // set is smaller than any other value.
            var u = FL.Min();
            var z = BL.Max();

            while (FL.Count > 0 && BL.Count > 0 && (u == z || nodeOrder.query(z, u)))
            {
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


                if (!F.Contains(x))
                {
                    F.Add(x);
                    x.Value.OutEnum = x.Value.outgoing.GetEnumerator();
                    x.Value.OutEnum.MoveNext();
                    if (x.Value.OutEnum.Current != null)
                        FL.Add(x);
                }
                if (!B.Contains(y))
                {
                    B.Add(y);
                    y.Value.InEnum = y.Value.incoming.GetEnumerator();
                    y.Value.InEnum.MoveNext();
                    if (y.Value.InEnum.Current != null)
                        BL.Add(y);
                }

                // End of SEARCH-STEP(vertex u, vertex z)
                u = FL.Min();
                z = BL.Max();
            }
            

            // let t = min({v}∪{x ∈ F|out(x) = null} and reorder the vertices in F< and B> as discussed previously.
            var vAndf = F.FindAll(item => item.Value.OutEnum.Current != null);
            vAndf.Add(v);
            var t = vAndf.Min();
            // Let F< = { x ∈ F | x < t} and 
            var fb = F.FindAll(item => item.label < t.label);
            // B > = { y ∈ B | y > t}. 
            var bf = B.FindAll(item => item.label > t.label);

            if(t == v)
            {
                DataStructures.SGTNode<HKMSTNode> prev;
                // move all vertices in fb just after t ... bf is empty            
                foreach (var node in fb)
                    nodeOrder.remove(node);
                                
                if(fb.Count > 1)
                    fb = topoSort(fb);

                if (fb.Count > 0)
                {
                    prev = nodeOrder.insertAfter(t, fb[0]);
                    for (int i = 1; i < fb.Count; i++)
                        prev = nodeOrder.insertAfter(prev, fb[i]);
                }                    
            }
            if (t.label < v.label)
            {
                DataStructures.SGTNode<HKMSTNode> prev;
                // move all vertices in fb just before t and all vertices in bf just before all vertices in fb

                //bf.Sort(); topologically if needed, try both
                if (bf.Count > 1)
                    bf = topoSort(bf);
                if (fb.Count > 1)
                    fb = topoSort(fb);

                foreach (var node in bf)
                    nodeOrder.remove(node);
                foreach (var node in fb)
                    nodeOrder.remove(node);

                foreach (var item in fb)
                    bf.Add(item);

                

                if (bf.Count > 0)
                {
                    prev = nodeOrder.insertBefore(t, bf[bf.Count-1]);
                    if(bf.Count > 1)
                    {
                        for (int i = bf.Count - 2; i >= 0; i--)
                            prev = nodeOrder.insertBefore(prev, bf[i]);
                    }                    
                }
            }

            // all done add to outgoing and incoming
            nodes[iv].Value.outgoing.Add(nodes[iw]);
            nodes[iw].Value.incoming.Add(nodes[iv]);

            return true; // null
        }

        List<DataStructures.SGTNode<HKMSTNode>> topoSort(List<DataStructures.SGTNode<HKMSTNode>> graph)
        {
            
            var dfs = new StaticGraph.GenericTarjan<int>();
            var list = new List<Tuple<int, DataStructures.SGTNode<HKMSTNode>>>();

            var dict = new Dictionary<DataStructures.SGTNode<HKMSTNode>, int>();

            for (int i = 0; i < graph.Count; i++)
            {
                dict.Add(graph[i], i);
                dfs.addVertex(graph[i].Value.n);
            }
            
            foreach(var node in graph)
            {
                foreach (var to in node.Value.outgoing)
                {
                    if(graph.Contains(to))
                        dfs.addEdge(dict[node], dict[to]);
                }                    
            }

            var topology = dfs.topoSort();

            var retList = new List<DataStructures.SGTNode<HKMSTNode>>();
            foreach(var item in topology)
            {
                retList.Add(graph[item]);
            }
            
            return retList;
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
            return FL.Min(item => item.label) > BL.Max(item => item.label); 
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
