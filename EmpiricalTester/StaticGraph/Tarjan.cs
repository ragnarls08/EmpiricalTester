using System.Collections.Generic;

namespace EmpiricalTester.StaticGraph
{
    internal class Tarjan : IStaticGraph
    {
        private List<TarjanNode> _graph = new List<TarjanNode>();
        private List<int> _l = new List<int>();
        private bool _cycle;

        public void AddVertex()
        {
            _graph.Add(new TarjanNode());
        }

        public void AddEdge(int v, int w)
        {
            _graph[v].Outgoing.Add(w);
            _graph[w].Incoming.Add(v);
        }

        public void ResetAll()
        {
            _graph.Clear();
            _l.Clear();
            _cycle = false;
        }

        public void RemoveEdge(int v, int w)
        {
            _graph[v].Outgoing.Remove(w);
            _graph[w].Incoming.Remove(v);
        }

        public int[] TopoSort()
        {
            _l.Clear();
            ClearVisited();
            _cycle = false;

            for(var i = 0; i < _graph.Count; i++)
            {
                Visit(i);
            }

            return _cycle ? null : _l.ToArray();
        }

        private void Visit(int n)
        {
            if (_graph[n].TemporaryVisit)
            {
                _cycle = true;
                return;
            }
                
            
            if(!_graph[n].Visited)
            {
                _graph[n].TemporaryVisit = true;

                foreach(int m in _graph[n].Outgoing)
                {
                    Visit(m);
                }

                _graph[n].Visited = true;
                _graph[n].TemporaryVisit = false;

                _l.Insert(0, n);                
            }
        }

        private void ClearVisited()
        {
            foreach(var node in _graph)
            {
                node.Visited = false;
                node.TemporaryVisit = false;
            }
        }
    }
}
