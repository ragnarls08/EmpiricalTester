using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpiricalTester.GraphGeneration
{
    /// <summary>
    /// Uses dfs to create a connectivity matrix between nodes
    /// </summary>
    public class ConnectivityGraph
    {
        List<Node> graph;

        public ConnectivityGraph()
        {
            graph = new List<Node>();
        }

        public void addVertex()
        {
            graph.Add(new Node());
        }

        public void addEdge(int v, int w)
        {
            graph[v].outgoing.Add(w);
        }
                
        public List<Tuple<int, List<int>>> generateConnectivityMatrix()
        {
            for(int i = 0; i < graph.Count; i++)
            {
                if(!graph[i].visited)
                    visit(i);
            }

            // TODO bad idea, duplicates cannot happen. fix
            for (int i = 0; i < graph.Count; i++)
            {
                graph[i].pathTo = graph[i].pathTo.Distinct().ToList();
            }
            
            List<Tuple<int, List<int>>> results = new List<Tuple<int, List<int>>>();

            for(int i = 0; i < graph.Count; i++)
            {
                results.Add(new Tuple<int, List<int>>(i, graph[i].pathTo));
            }

            return results;
        }

        private List<int> visit(int v)
        {
            if (graph[v].visited)
                return graph[v].pathTo;

            graph[v].visited = true;
            
            for(int i = 0; i < graph[v].outgoing.Count; i++)
            {
                graph[v].pathTo.Add(graph[v].outgoing[i]);
                List<int> childPath = visit(graph[v].outgoing[i]);
                graph[v].addConnection(childPath);
            }

            return graph[v].pathTo;
        }

        protected class Node
        {
            public List<int> pathTo;
            public List<int> outgoing;
            public bool visited { get; set; }

            public void addConnection(List<int> nodes)
            {
                pathTo = pathTo.Concat(nodes).ToList<int>();
            }

            public Node()
            {
                pathTo = new List<int>();
                outgoing = new List<int>();
            }                       
        }
    }
}
