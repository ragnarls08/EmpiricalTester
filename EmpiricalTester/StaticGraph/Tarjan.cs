using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpiricalTester.StaticGraph
{
    class Tarjan : IStaticGraph
    {
        private List<TarjanNode> graph = new List<TarjanNode>();
        private List<int> L = new List<int>();
        private bool cycle = false;

        public void addVertex()
        {
            graph.Add(new TarjanNode());
        }

        public void addEdge(int v, int w)
        {
            graph[v].outgoing.Add(w);
            graph[w].incoming.Add(v);
        }

        public int[] topoSort()
        {
            L.Clear();

            for(int i = graph.FindIndex(item => item.visited == false); i >= 0; i = graph.FindIndex(item => item.visited == false))
            {
                visit(i);
            }

            if (cycle)
                return null;
            else
                return L.ToArray();
        }

        private void visit(int n)
        {
            if (graph[n].temporaryVisit)
            {
                cycle = true;
                return;
            }
                
            
            if(!graph[n].visited)
            {
                graph[n].temporaryVisit = true;

                foreach(int m in graph[n].outgoing)
                {
                    visit(m);
                }

                graph[n].visited = true;
                graph[n].temporaryVisit = false;

                L.Insert(0, n);                
            }

        }
    }
}
