using System.Collections.Generic;
using System.Linq;

namespace EmpiricalTester.DynamicGraph
{
    public class HKMSTV1 : IDynamicGraph
    {        
        private DataStructures.SGTree<HKMSTNode> _nodeOrder;
        private List<DataStructures.SGTNode<HKMSTNode>> _nodes;

        private List<DataStructures.SGTNode<HKMSTNode>> f;
        private List<DataStructures.SGTNode<HKMSTNode>> b;

        private int _nCount;

        public HKMSTV1(double alpha)
        {
            _nodeOrder = new DataStructures.SGTree<HKMSTNode>(alpha);
            _nodes = new List<DataStructures.SGTNode<HKMSTNode>>();


            f = new List<DataStructures.SGTNode<HKMSTNode>>();
            b = new List<DataStructures.SGTNode<HKMSTNode>>();            
        }

        public bool AddEdge(int v, int w)
        {
            // When a new arc (v, w) has v>w, do the search by calling VERTEX-GUIDEDSEARCH(v,w)
            if (_nodeOrder.Query(_nodes[v], _nodes[w]))
                return VertexGuidedSearch(v, w);
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
                ? _nodeOrder.insertFirst(new HKMSTNode(_nCount))
                : _nodeOrder.insertAfter(_nodeOrder.Root, new HKMSTNode(_nCount)));

            _nCount++;
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


        private bool VertexGuidedSearch(int iv, int iw)
        {
            var v = _nodes[iv];
            var w = _nodes[iw];

            // Article states double linked list for F,B and normal list for FL BL
            //var f = new List<DataStructures.SGTNode<HKMSTNode>>();
            //var b = new List<DataStructures.SGTNode<HKMSTNode>>();
            f.Clear();
            b.Clear();

            f.Add(w);
            w.Value.InF = true;
            b.Add(v);
            v.Value.InB = true;
         
            w.Value.OutEnum = w.Value.Outgoing.GetEnumerator();
            w.Value.OutEnum.MoveNext();
            v.Value.InEnum = v.Value.Incoming.GetEnumerator();
            v.Value.InEnum.MoveNext();
        
            var fl = new C5.IntervalHeap<DataStructures.SGTNode<HKMSTNode>>();
            var bl = new C5.IntervalHeap<DataStructures.SGTNode<HKMSTNode>>();
            
            if (w.Value.OutEnum.Current != null)
                fl.Add(w);
            if (v.Value.InEnum.Current != null)
                bl.Add(v);

            // For ease of notation, we adopt the convention that the
            // minimum of an empty set is bigger than any other value and the maximum of an empty
            // set is smaller than any other value.
            DataStructures.SGTNode<HKMSTNode> u = null;
            DataStructures.SGTNode<HKMSTNode> z = null;
            if(fl.Count > 0)
                u = fl.FindMin();
            if(bl.Count > 0)
                z = bl.FindMax();
            
            while (fl.Count > 0 && bl.Count > 0 && (u == z || _nodeOrder.Query(z, u)))
            {
                // SEARCH-STEP(vertex u, vertex z)
              
                var x = u.Value.OutEnum.Current;
                var y = z.Value.InEnum.Current;
                u.Value.OutEnum.MoveNext();
                z.Value.InEnum.MoveNext();

                if (u.Value.OutEnum.Current == null)
                    fl.DeleteMin();
                if (z.Value.InEnum.Current == null)
                    bl.DeleteMax();

 
                
                if(x.Value.InB)
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
                        fl.Add(x);
                }
                if (!y.Value.InB)                
                {
                    b.Add(y);
                    y.Value.InB = true;
                    y.Value.InEnum = y.Value.Incoming.GetEnumerator();
                    y.Value.InEnum.MoveNext();
                    if (y.Value.InEnum.Current != null)
                        bl.Add(y);
                }

                // End of SEARCH-STEP(vertex u, vertex z)
                if (fl.Count > 0)
                    u = fl.FindMin();
                if (bl.Count > 0)
                    z = bl.FindMax();
                
            }

            
            // let t = min({v}∪{x ∈ F|out(x) = null} and reorder the vertices in F< and B> as discussed previously.
            var vAndf = f.FindAll(item => item.Value.OutEnum.Current != null);
            vAndf.Add(v);
            var t = vAndf.Min();
            // Let F< = { x ∈ F | x < t} and 
            var fb = f.FindAll(item => item.Label < t.Label);
            // B > = { y ∈ B | y > t}. 
            var bf = b.FindAll(item => item.Label > t.Label);

            if(t == v)
            {
                // move all vertices in fb just after t ... bf is empty            
                foreach (var node in fb)
                    _nodeOrder.Remove(node);
                                
                if(fb.Count > 1)
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

                foreach (var item in fb)
                    bf.Add(item);
                
                if (bf.Count > 0)
                {
                    var prev = _nodeOrder.insertBefore(t, bf[bf.Count-1]);
                    if(bf.Count > 1)
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

            return true; // null
        }              
        
        List<DataStructures.SGTNode<HKMSTNode>> TopoSort(List<DataStructures.SGTNode<HKMSTNode>> graph)            
        {
            var dfs = new StaticGraph.GenericTarjan<int>();
            var dict = new Dictionary<DataStructures.SGTNode<HKMSTNode>, int>();

            for (int i = 0; i < graph.Count; i++)
            {
                dict.Add(graph[i], i);
                dfs.AddVertex(graph[i].Value.N);
            }
            
            foreach(var node in graph)
            {
                foreach (var to in node.Value.Outgoing)
                {
                    if(graph.Contains(to))
                        dfs.AddEdge(dict[node], dict[to]);
                }                    
            }

            var topology = dfs.TopoSort();

            return topology.Select(item => graph[item]).ToList();
        }
    }
}
