using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpiricalTester.DynamicGraph
{
    class SimpleIncremental : IDynamicGraph
    {
        private List<SimpleNode> graph;

        public SimpleIncremental()
        {
            graph = new List<SimpleNode>();
        }

        public void addVertex()
        {
            graph.Add(new SimpleNode());
        }

        public void removeEdge(int v, int w)
        {
            graph[w].incoming.Remove(v);
        }

        public void resetAll()
        {
            this.graph.Clear();
        }

        public bool addEdge(int v, int w)
        {            
            graph[w].incoming.Add(v);
            graph[w].blackHole = true;

            if(!visit(graph[v], graph[w].level))
            {
                removeEdge(v, w);
                graph[w].blackHole = false;
                graph.ForEach(item => item.visited = false);
                return false;
            }

            
            graph[w].blackHole = false;
            graph.ForEach(item => item.visited = false);
            return true;
        }
               
        public List<int> topology()
        {
            // use select to get (index, level) pair, order the result, convert to list of indexes ordered by topo (level)
            return graph.Select((Value, Index) => new { Index, Value.level }).OrderByDescending(item => item.level).ToList().ConvertAll(item => item.Index);
        }

        private bool visit(SimpleNode v, int childLevel)
        {
            int oldLevel = v.level;
            v.visited = true;

            if(v.blackHole)
            {
                return false;
            }

            if (v.level <= childLevel)
            {
                v.level = childLevel + 1;
            }
            
            for(int i = 0; i < v.incoming.Count; i++)
            {
                if(!graph[v.incoming[i]].visited)
                {
                    if (!visit(graph[v.incoming[i]], v.level))
                    {
                        v.level = oldLevel;
                        return false;
                    }
                }                
            }
            
            return true;
        }
    }
}
