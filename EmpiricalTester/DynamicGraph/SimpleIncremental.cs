using System.Collections.Generic;
using System.Linq;

namespace EmpiricalTester.DynamicGraph
{
    public class SimpleIncremental : IDynamicGraph
    {
        private List<SimpleNode> _graph;

        public SimpleIncremental()
        {
            _graph = new List<SimpleNode>();
        }

        public void AddVertex()
        {
            _graph.Add(new SimpleNode());
        }

        public void RemoveEdge(int v, int w)
        {
            _graph[w].Incoming.Remove(v);
        }

        public void ResetAll(int newN)
        {
            _graph.Clear();
        }

        public void ResetAll(int newN, int newM)
        {
            ResetAll(newN);
        }

        public bool AddEdge(int v, int w)
        {            
            _graph[w].Incoming.Add(v);
            _graph[w].BlackHole = true;

            if(!Visit(_graph[v], _graph[w].Level))
            {
                RemoveEdge(v, w);
                _graph[w].BlackHole = false;                
                return false;
            }
                        
            _graph[w].BlackHole = false;
            
            return true;
        }
               
        public List<int> Topology()
        {
            // use select to get (index, level) pair, order the result, convert to list of indexes ordered by topo (level)
            return _graph.Select((value, index) => new { Index = index, level = value.Level }).OrderByDescending(item => item.level).ToList().ConvertAll(item => item.Index);
        }

        private bool Visit(SimpleNode v, int childLevel)
        {
            int oldLevel = v.Level;

            if(v.BlackHole)
            {
                return false;
            }

            if (v.Level <= childLevel)
            {
                v.Level = childLevel + 1;

                if (v.Incoming.Any(t => !Visit(_graph[t], v.Level)))
                {
                    v.Level = oldLevel;
                    return false;
                }
            }

            return true;
        }
    }
}
