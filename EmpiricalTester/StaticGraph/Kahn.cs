using System.Collections.Generic;

namespace EmpiricalTester.StaticGraph
{
    internal class Kahn : IStaticGraph
    {
        private List<KahnNode> _graph = new List<KahnNode>();
        private List<KahnNode> _graphTemp = new List<KahnNode>();
        private int _edgeCount;
        private int _edgeCountTemp;

        public void AddVertex()
        {
            _graph.Add(new KahnNode());
        }

        public void ResetAll()
        {
            _graph.Clear();
            _graphTemp.Clear();
            _edgeCount = 0;
            _edgeCountTemp = 0;
        }

        public void AddEdge(int v, int w)
        {
            _graph[v].Outgoing.Add(w);
            _graph[w].Incoming.Add(v);

            _edgeCount++;
        }

        public void RemoveEdge(int v, int w)
        {
            _graph[v].Outgoing.Remove(w);
            _graph[w].Incoming.Remove(v);

            _edgeCount--;
        }

        public int[] TopoSort()
        {
            _graphTemp = DeepCopy(_graph);
            _edgeCountTemp = _edgeCount;

            var l = new List<int>(); // sorted output
            var s = new List<int>(); // all nodes with no Incoming

            for(var i = 0; i < _graph.Count; i++)
            {
                if(_graph[i].Incoming.Count == 0)
                {
                    s.Add(i);
                }
            }

            while(s.Count > 0)
            {
                var n = s[0];
                s.RemoveAt(0);
                l.Add(n);

                // for each node m with an edge e from n to m do
                var nOutgoing = new List<int>(_graph[n].Outgoing); // copied since items are removed.
                foreach (int m in nOutgoing)
                {
                    // remove edge e from the _graph
                    RemoveEdge(n, m);

                    if(_graph[m].Incoming.Count == 0)
                    {
                        s.Add(m);
                    }
                }
            }

            if (_edgeCount > 0)
            {
                _graph = DeepCopy(_graphTemp);
                _edgeCount = _edgeCountTemp;
                return null;
            }

            _graph = DeepCopy(_graphTemp);
            _edgeCount = _edgeCountTemp;
            return l.ToArray();    
        }

        private List<KahnNode> DeepCopy(List<KahnNode> toCopy)
        {
            return toCopy.ConvertAll(item => new KahnNode(item));
        }
    }
}
