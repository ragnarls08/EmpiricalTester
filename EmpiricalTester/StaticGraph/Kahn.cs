using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpiricalTester.StaticGraph
{
    class Kahn : IStaticGraph
    {
        private List<KahnNode> graph = new List<KahnNode>();
        private List<KahnNode> graph_temp = new List<KahnNode>();
        private int edgeCount = 0;
        private int edgeCount_temp = 0;

        public void addVertex()
        {
            graph.Add(new KahnNode());
        }

        public void resetAll()
        {
            graph.Clear();
            graph_temp.Clear();
            edgeCount = 0;
            edgeCount_temp = 0;
        }

        public void addEdge(int v, int w)
        {
            graph[v].outgoing.Add(w);
            graph[w].incoming.Add(v);

            edgeCount++;
        }

        public void removeEdge(int v, int w)
        {
            graph[v].outgoing.Remove(w);
            graph[w].incoming.Remove(v);

            edgeCount--;
        }

        public int[] topoSort()
        {
            graph_temp = deepCopy(graph);
            edgeCount_temp = edgeCount;

            List<int> L = new List<int>(); // sorted output
            List<int> S = new List<int>(); // all nodes with no incoming

            for(int i = 0; i < graph.Count; i++)
            {
                if(graph[i].incoming.Count == 0)
                {
                    S.Add(i);
                }
            }

            while(S.Count > 0)
            {
                int n = S[0];
                S.RemoveAt(0);
                L.Add(n);

                // for each node m with an edge e from n to m do
                List<int> nOutgoing = new List<int>(graph[n].outgoing); // copied since items are removed.
                for (int i = 0; i < nOutgoing.Count; i++)
                {
                    int m = nOutgoing[i];

                    // remove edge e from the graph
                    removeEdge(n, m);

                    if(graph[m].incoming.Count == 0)
                    {
                        S.Add(m);
                    }

                }
            }

            if (edgeCount > 0)
            {
                graph = deepCopy(graph_temp);
                edgeCount = edgeCount_temp;
                return null;
            }
            else
            {
                graph = deepCopy(graph_temp);
                edgeCount = edgeCount_temp;
                return L.ToArray();
            }
                
        }

        private List<KahnNode> deepCopy(List<KahnNode> toCopy)
        {
            return toCopy.ConvertAll(item => new KahnNode(item));
        }
    }
}
