using System;
using System.Collections.Generic;
using System.Linq;

namespace EmpiricalTester.DynamicGraph 
{

    public enum SPickMethod { Random, MoMRandom, MoM };
    public class HKMSTFinal : IDynamicGraph
    {
        private DataStructures.SGTree<HKMSTNodeFinal> _nodeOrder;
        private List<DataStructures.SGTNode<HKMSTNodeFinal>> _nodes;
        private SPickMethod _sPickMethod;

        private readonly Random _r = new Random(DateTime.Now.Millisecond);
        private int _nCount;//debug

        public HKMSTFinal(double alpha, SPickMethod spick)
        {
            _nodeOrder = new DataStructures.SGTree<HKMSTNodeFinal>(alpha);
            _nodes = new List<DataStructures.SGTNode<HKMSTNodeFinal>>();
            _sPickMethod = spick;
        }

        public bool AddEdge(int v, int w)
        {
            // When a new arc (v, w) has v>w, do the search by calling VERTEX-GUIDEDSEARCH(v,w)
            if (_nodeOrder.Query(_nodes[v], _nodes[w]))
                return SoftThresholdSearch(v, w);
            else
            {
                _nodes[v].Value.Outgoing.Add(_nodes[w]);
                _nodes[w].Value.Incoming.Add(_nodes[v]);
            }

            return true;            
        }

        public void AddVertex()
        {
            _nodes.Add(_nodeOrder.Root == null
                ? _nodeOrder.insertFirst(new HKMSTNodeFinal(_nCount))
                : _nodeOrder.insertAfter(_nodeOrder.Root, new HKMSTNodeFinal(_nCount)));

            _nCount++;
        }

        public bool SoftThresholdSearch(int iv, int iw)
        {
            var v = _nodes[iv];
            var w = _nodes[iw];

            // Article states double linked list for F,B and normal list for FL BL
            var f = new List<DataStructures.SGTNode<HKMSTNodeFinal>>(1);
            var b = new List<DataStructures.SGTNode<HKMSTNodeFinal>>(1);
            f.Add(w);
            w.Value.InF = true;
            b.Add(v);
            v.Value.InB = true;

            w.Value.OutEnum = w.Value.Outgoing.GetEnumerator();
            w.Value.OutEnum.MoveNext();
            v.Value.InEnum = v.Value.Incoming.GetEnumerator();
            v.Value.InEnum.MoveNext();

            DataStructures.SGTNode<HKMSTNodeFinal> s = v;

            var fa = new LinkedList<DataStructures.SGTNode<HKMSTNodeFinal>>();// should be linked list?
            var fp = new LinkedList<DataStructures.SGTNode<HKMSTNodeFinal>>();
            var ba = new LinkedList<DataStructures.SGTNode<HKMSTNodeFinal>>();
            var bp = new LinkedList<DataStructures.SGTNode<HKMSTNodeFinal>>();
            
            if (w.Value.OutEnum.Current != null)
                fa.AddFirst(w);
                             
            if (v.Value.InEnum.Current != null)
                ba.AddFirst(v);

            while (fa.Count > 0 && ba.Count > 0)
            {
                var u = fa.First; // pointer to linkedlistnode for faster remove
                var z = ba.First;

                if(_nodeOrder.Query(z.Value, u.Value) || u.Value == z.Value)
                {
                    // SEARCH_STEP(u,z)
                    var x = u.Value.Value.OutEnum.Current;
                    var y = z.Value.Value.InEnum.Current;
                    u.Value.Value.OutEnum.MoveNext();
                    z.Value.Value.InEnum.MoveNext();

                    if (u.Value.Value.OutEnum.Current == null)
                        fa.Remove(u);
                    if (z.Value.Value.InEnum.Current == null)
                        ba.Remove(z);
                    
                    if (x.Value.InB)
                    {
                        f.ForEach(item => item.Value.InF = false);
                        b.ForEach(item => item.Value.InB = false);
                        return false; // Pair(uz.from, x.Current);
                    }
                    else if (y.Value.InF)
                    {
                        f.ForEach(item => item.Value.InF = false);
                        b.ForEach(item => item.Value.InB = false);
                        return false; // Pair(y.Current, uz.to);
                    }


                    if (!x.Value.InF)
                    {
                        f.Add(x);
                        x.Value.InF = true;
                        x.Value.OutEnum = x.Value.Outgoing.GetEnumerator();
                        x.Value.OutEnum.MoveNext();
                        if (x.Value.OutEnum.Current != null)
                            fa.AddFirst(x);
                    }
                    if (!y.Value.InB)
                    {
                        b.Add(y);
                        y.Value.InB = true;
                        y.Value.InEnum = y.Value.Incoming.GetEnumerator();
                        y.Value.InEnum.MoveNext();
                        if (y.Value.InEnum.Current != null)
                            ba.AddFirst(y);
                    }
                    // END STEP
                }
                else
                {
                    if(_nodeOrder.Query(u.Value, s))
                    {
                        fa.Remove(u);
                        fp.AddFirst(u);
                    }
                    if(_nodeOrder.Query(s, z.Value))
                    {
                        ba.Remove(z);
                        bp.AddFirst(z);
                    }
                }

                if(fa.Count == 0)
                {
                    bp.Clear();
                    ba.Remove(s);
                    if(fp.Count > 0)
                    {
                        //choose s from Fp (median stuff)
                        s = PickS(fp);
                        
                        fa.Clear();
                        var curr = fp.First;
                        do
                        {
                            var next = curr.Next;
                            if(curr.Value.Label <= s.Label)
                            {
                                fp.Remove(curr);
                                fa.AddFirst(curr.Value);
                            }
                            curr = next;
                        } while (curr != null);
                    }
                }
                if(ba.Count == 0)
                {
                    fp.Clear();
                    fa.Remove(s);
                    if(bp.Count > 0)
                    {
                        //choose s from Bp (median stuff)
                        s = PickS(bp);

                        ba.Clear();
                        var curr = bp.First;
                        do
                        {
                            var next = curr.Next;
                            if (curr.Value.Label >= s.Label)
                            {
                                bp.Remove(curr);
                                ba.AddFirst(curr.Value);                                
                            }
                            curr = next;
                        } while (curr != null);                  
                    }
                }            
            }
            // reorder
            // let t = min({v}∪{x ∈ F|out(x) = null} and reorder the vertices in F< and B> as discussed previously.
            
            var vAndf = f.FindAll(item => item.Value.OutEnum.Current != null);
            vAndf.Add(v);
            var t = vAndf.Min();
            // Let F< = { x ∈ F | x < t} and 
            var fb = f.FindAll(item => item.Label < t.Label);
            // B > = { y ∈ B | y > t}. 
            var bf = b.FindAll(item => item.Label > t.Label);

            if (t == v)
            {
                // move all vertices in fb just after t ... bf is empty            
                foreach (var node in fb)
                    _nodeOrder.Remove(node);

                if (fb.Count > 1)
                    fb = TopoSort(fb);

                if (fb.Count > 0)
                {
                    var prev = _nodeOrder.insertAfter(t, fb[0]);
                    for (int i = 1; i < fb.Count; i++)
                        prev = _nodeOrder.insertAfter(prev, fb[i]);
                }
            }
            if (t.Label < v.Label)
            {
                // move all vertices in fb just before t and all vertices in bf just before all vertices in fb

                //bf.Sort(); topologically if needed, try both
                if (bf.Count > 1)
                    bf = TopoSort(bf);
                if (fb.Count > 1)
                    fb = TopoSort(fb);

                foreach (var node in bf)
                    _nodeOrder.Remove(node);
                foreach (var node in fb)
                    _nodeOrder.Remove(node);

                bf.AddRange(fb);

                if (bf.Count > 0)
                {
                    var prev = _nodeOrder.insertBefore(t, bf[bf.Count - 1]);
                    if (bf.Count > 1)
                    {
                        for (int i = bf.Count - 2; i >= 0; i--)
                            prev = _nodeOrder.insertBefore(prev, bf[i]);
                    }
                }
            }


            // reset bools
            f.ForEach(item => item.Value.InF = false);
            b.ForEach(item => item.Value.InB = false);

            // all done add to Outgoing and Incoming
            _nodes[iv].Value.Outgoing.Add(_nodes[iw]);
            _nodes[iw].Value.Incoming.Add(_nodes[iv]);
            return true;
        }


        List<DataStructures.SGTNode<HKMSTNodeFinal>> TopoSort(List<DataStructures.SGTNode<HKMSTNodeFinal>> graph)
        {
            var dfs = new StaticGraph.GenericTarjan<int>();
            var dict = new Dictionary<DataStructures.SGTNode<HKMSTNodeFinal>, int>();

            for (int i = 0; i < graph.Count; i++)
            {
                dict.Add(graph[i], i);
                dfs.AddVertex(graph[i].Value.N);
            }

            foreach (var node in graph)
            {
                foreach (var to in node.Value.Outgoing)
                {
                    if (graph.Contains(to))
                        dfs.AddEdge(dict[node], dict[to]);
                }
            }

            var topology = dfs.TopoSort();

            return topology.Select(item => graph[item]).ToList();
        }


        public void RemoveEdge(int v, int w)
        {
            //graph[w].Incoming.Remove(v);
            //graph[w].Outgoing.Remove(v);
        }

        public void ResetAll(int newN)
        {
            _nCount = 0;
            _nodeOrder.Clear();
            _nodes.Clear();
        }

        public List<int> Topology()
        {
            return _nodeOrder.inOrder().ConvertAll(item => item.N);
        }

        private DataStructures.SGTNode<HKMSTNodeFinal> PickS(LinkedList<DataStructures.SGTNode<HKMSTNodeFinal>> list)
        {
            switch (_sPickMethod)
            {
                case SPickMethod.Random:                    
                    return list.ElementAt(_r.Next(0, list.Count -1));
                case SPickMethod.MoMRandom:
                    return null;
                default:
                    return Algorithms.Median.QuickSelect(list.ToList(), list.Count / 2,
                            Comparer<DataStructures.SGTNode<HKMSTNodeFinal>>.Create((a, b) => a.Label > b.Label ? 1 : a.Label < b.Label ? -1 : 0));

            }
        }
    }
}
