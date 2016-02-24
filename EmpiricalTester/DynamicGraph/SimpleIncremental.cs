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

        public bool addEdge(int v, int w)
        {            
            graph[w].incoming.Add(v);
            graph[w].blackHole = true;

            if(!visit(graph[v], graph[w].level))
            {
                removeEdge(v, w);
                graph[w].blackHole = false;
                return false;
            }


            graph[w].blackHole = false;
            return true;
        }

        public void removeEdge(int v, int w)
        {
            graph[w].incoming.Remove(v);
        }

   

        private bool visit(SimpleNode v, int childLevel)
        {
            if(v.level <= childLevel)
            {
                v.level = childLevel + 1;
            }

            if(v.blackHole)
            {
                return false;
            }

            foreach(int parent in v.incoming)
            {
                if (!visit(graph[parent], v.level))
                    return false;
            }

            return true;
        }

        public List<int> topology()
        {
            // use select to get (index, level) pair, order the result, convert to list of indexes ordered by topo (level)
            return graph.Select((Value, Index) => new { Index, Value.level }).OrderBy(item => item.level).ToList().ConvertAll(item => item.Index);
        }
    }
}
