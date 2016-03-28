using System;
using System.Collections.Generic;
using System.Linq;

namespace EmpiricalTester.DynamicGraph 
{
    public class HKMST_Final : IDynamicGraph
    {
        private DataStructures.SGTree<HKMSTNodeFinal> nodeOrder;
        private List<DataStructures.SGTNode<HKMSTNodeFinal>> nodes;

        private int nCount = 0;//debug

        public HKMST_Final(double alpha)
        {
            nodeOrder = new DataStructures.SGTree<HKMSTNodeFinal>(alpha);
            nodes = new List<DataStructures.SGTNode<HKMSTNodeFinal>>();
        }

        public bool addEdge(int v, int w)
        {
            // When a new arc (v, w) has v>w, do the search by calling VERTEX-GUIDEDSEARCH(v,w)
            if (nodeOrder.query(nodes[v], nodes[w]))
                return softThresholdSearch(v, w);
            else
            {
                nodes[v].Value.outgoing.Add(nodes[w]);
                nodes[w].Value.incoming.Add(nodes[v]);
            }

            return true;            
        }

        public void addVertex()
        {
            if (nodeOrder.Root == null)
                nodes.Add(nodeOrder.insertFirst(new HKMSTNodeFinal(nCount)));
            else
                nodes.Add(nodeOrder.insertAfter(nodeOrder.Root, new HKMSTNodeFinal(nCount)));

            nCount++;
        }

        public bool softThresholdSearch(int iv, int iw)
        {
            var v = nodes[iv];
            var w = nodes[iw];

            // Article states double linked list for F,B and normal list for FL BL
            var F = new List<DataStructures.SGTNode<HKMSTNodeFinal>>(1);
            var B = new List<DataStructures.SGTNode<HKMSTNodeFinal>>(1);
            F.Add(w);
            w.Value.InF = true;
            B.Add(v);
            v.Value.InB = true;

            w.Value.OutEnum = w.Value.outgoing.GetEnumerator();
            w.Value.OutEnum.MoveNext();
            v.Value.InEnum = v.Value.incoming.GetEnumerator();
            v.Value.InEnum.MoveNext();

            DataStructures.SGTNode<HKMSTNodeFinal> s = v;

            var Fa = new LinkedList<DataStructures.SGTNode<HKMSTNodeFinal>>();// should be linked list?
            var Fp = new LinkedList<DataStructures.SGTNode<HKMSTNodeFinal>>();
            var Ba = new LinkedList<DataStructures.SGTNode<HKMSTNodeFinal>>();
            var Bp = new LinkedList<DataStructures.SGTNode<HKMSTNodeFinal>>();
            
            if (w.Value.OutEnum.Current != null)
                Fa.AddFirst(w);
                             
            if (v.Value.InEnum.Current != null)
                Ba.AddFirst(v);

            while (Fa.Count > 0 && Ba.Count > 0)
            {
                var u = Fa.First.Value;
                var z = Ba.First.Value;

                if(nodeOrder.query(z, u) || u == z)
                {
                    // SEARCH_STEP(u,z)
                    var x = u.Value.OutEnum.Current;
                    var y = z.Value.InEnum.Current;
                    u.Value.OutEnum.MoveNext();
                    z.Value.InEnum.MoveNext();

                    if (u.Value.OutEnum.Current == null)
                        Fa.Remove(u);
                    if (z.Value.InEnum.Current == null)
                        Ba.Remove(z);
                    
                    if (x.Value.InB)
                    {
                        F.ForEach(item => item.Value.InF = false);
                        B.ForEach(item => item.Value.InB = false);
                        return false; // Pair(uz.from, x.Current);
                    }
                    else if (y.Value.InF)
                    {
                        F.ForEach(item => item.Value.InF = false);
                        B.ForEach(item => item.Value.InB = false);
                        return false; // Pair(y.Current, uz.to);
                    }


                    if (!x.Value.InF)
                    {
                        F.Add(x);
                        x.Value.InF = true;
                        x.Value.OutEnum = x.Value.outgoing.GetEnumerator();
                        x.Value.OutEnum.MoveNext();
                        if (x.Value.OutEnum.Current != null)
                            Fa.AddFirst(x);
                    }
                    if (!y.Value.InB)
                    {
                        B.Add(y);
                        y.Value.InB = true;
                        y.Value.InEnum = y.Value.incoming.GetEnumerator();
                        y.Value.InEnum.MoveNext();
                        if (y.Value.InEnum.Current != null)
                            Ba.AddFirst(y);
                    }
                    // END STEP
                }
                else
                {
                    if(nodeOrder.query(u, s))
                    {
                        Fa.Remove(u);
                        Fp.AddFirst(u);
                    }
                    if(nodeOrder.query(s, z))
                    {
                        Ba.Remove(z);
                        Bp.AddFirst(z);
                    }
                }

                if(Fa.Count == 0)
                {
                    Bp.Clear();
                    Ba.Remove(s);
                    if(Fp.Count > 0)
                    {
                        //choose s from Fp (median stuff)
                        s = Algorithms.Median.QuickSelect(Fp.ToList(), Fp.Count / 2, 
                            Comparer<DataStructures.SGTNode<HKMSTNodeFinal>>.Create((a,b) => a.label > b.label ? 1: a.label < b.label ? -1 : 0));
                        
                        Fa.Clear();
                        var curr = Fp.First;
                        do
                        {
                            var next = curr.Next;
                            if(curr.Value.label <= s.label)
                            {
                                Fp.Remove(curr);
                                Fa.AddFirst(curr.Value);                                
                            }
                            curr = next;
                        } while (curr != null);                       
                    }
                }
                if(Ba.Count == 0)
                {
                    Fp.Clear();
                    Fa.Remove(s);
                    if(Bp.Count > 0)
                    {
                        //choose s from Fp (median stuff)
                        s = Algorithms.Median.QuickSelect(Bp.ToList(), Bp.Count / 2,
                            Comparer<DataStructures.SGTNode<HKMSTNodeFinal>>.Create((a, b) => a.label > b.label ? 1 : a.label < b.label ? -1 : 0));


                        Ba.Clear();
                        var curr = Bp.First;
                        do
                        {
                            var next = curr.Next;
                            if (curr.Value.label >= s.label)
                            {
                                Bp.Remove(curr);
                                Ba.AddFirst(curr.Value);                                
                            }
                            curr = next;
                        } while (curr != null);                  
                    }
                }            
            }
            // reorder
            // let t = min({v}∪{x ∈ F|out(x) = null} and reorder the vertices in F< and B> as discussed previously.
            
            var vAndf = F.FindAll(item => item.Value.OutEnum.Current != null);
            vAndf.Add(v);
            var t = vAndf.Min();
            // Let F< = { x ∈ F | x < t} and 
            var fb = F.FindAll(item => item.label < t.label);
            // B > = { y ∈ B | y > t}. 
            var bf = B.FindAll(item => item.label > t.label);

            if (t == v)
            {
                DataStructures.SGTNode<HKMSTNodeFinal> prev;
                // move all vertices in fb just after t ... bf is empty            
                foreach (var node in fb)
                    nodeOrder.remove(node);

                if (fb.Count > 1)
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
                DataStructures.SGTNode<HKMSTNodeFinal> prev;
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
                    prev = nodeOrder.insertBefore(t, bf[bf.Count - 1]);
                    if (bf.Count > 1)
                    {
                        for (int i = bf.Count - 2; i >= 0; i--)
                            prev = nodeOrder.insertBefore(prev, bf[i]);
                    }
                }
            }


            // reset bools
            F.ForEach(item => item.Value.InF = false);
            B.ForEach(item => item.Value.InB = false);

            // all done add to outgoing and incoming
            nodes[iv].Value.outgoing.Add(nodes[iw]);
            nodes[iw].Value.incoming.Add(nodes[iv]);
            return true;
        }


        List<DataStructures.SGTNode<HKMSTNodeFinal>> topoSort(List<DataStructures.SGTNode<HKMSTNodeFinal>> graph)
        {
            //graph.Sort();
            //return graph;
            ///
            var dfs = new StaticGraph.GenericTarjan<int>();
            var list = new List<Tuple<int, DataStructures.SGTNode<HKMSTNodeFinal>>>();

            var dict = new Dictionary<DataStructures.SGTNode<HKMSTNodeFinal>, int>();

            for (int i = 0; i < graph.Count; i++)
            {
                dict.Add(graph[i], i);
                dfs.addVertex(graph[i].Value.n);
            }

            foreach (var node in graph)
            {
                foreach (var to in node.Value.outgoing)
                {
                    if (graph.Contains(to))
                        dfs.addEdge(dict[node], dict[to]);
                }
            }

            var topology = dfs.topoSort();

            var retList = new List<DataStructures.SGTNode<HKMSTNodeFinal>>();
            foreach (var item in topology)
            {
                retList.Add(graph[item]);
            }            

            return retList;
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
    }
}
