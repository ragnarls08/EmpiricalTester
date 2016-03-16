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
                
        public List<List<bool>> generateConnectivityMatrix()
        {
            List<List<int>> dist = new List<List<int>>(this.graph.Count);
            for(int i = 0; i < graph.Count; i++)
            {
                dist.Add(new List<int>(graph.Count));
                for (int x = 0; x < graph.Count; x++)
                {
                    dist[i].Add((i == x ? 0 : (int.MaxValue/2)-1)); // max + max = -2
                }                    
            }

            for(int i = 0; i < graph.Count; i++)
            {
                foreach(var to in graph[i].outgoing)
                {
                    dist[i][to] = 1;
                }
            }

            // Floyd–Warshall algorithm
            for (int k = 0; k < graph.Count; k++)
            {
                for(int i = 0; i < graph.Count; i++)
                {
                    for(int j = 0; j < graph.Count; j++)
                    {
                        if (dist[i][j] > dist[i][k] + dist[k][j])
                            dist[i][j] = dist[i][k] + dist[k][j];
                    }
                }
            }

            var results = new List<List<bool>>();

            for (int i = 0; i < graph.Count; i++)
            {
                results.Add(new List<bool>());               

                for (int x = 0; x < graph.Count; x++)
                {
                    if (i == x)
                        results[i].Add(false);
                    else if (dist[i][x] < (int.MaxValue/2) - 1)
                        results[i].Add(true);
                    else
                        results[i].Add(false);
                }
                
            }

            return results;
        }
              
        protected class Node
        {
            public List<int> outgoing;
            public bool visited { get; set; }

            public Node()
            {
                outgoing = new List<int>();
            }                       
        }
    }
}
