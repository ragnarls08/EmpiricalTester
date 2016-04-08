using System.Collections.Generic;

namespace EmpiricalTester.GraphGeneration
{
    /// <summary>
    /// Uses dfs to create a connectivity matrix between nodes
    /// </summary>
    public class ConnectivityGraph
    {
        List<Node> _graph;

        public ConnectivityGraph()
        {
            _graph = new List<Node>();
        }

        public void AddVertex()
        {
            _graph.Add(new Node());
        }

        public void AddEdge(int v, int w)
        {
            _graph[v].Outgoing.Add(w);
        }
                
        public List<List<bool>> GenerateConnectivityMatrix()
        {
            List<List<int>> dist = new List<List<int>>(_graph.Count);
            for(int i = 0; i < _graph.Count; i++)
            {
                dist.Add(new List<int>(_graph.Count));
                for (int x = 0; x < _graph.Count; x++)
                {
                    dist[i].Add(i == x ? 0 : int.MaxValue / 2 - 1); // max + max = -2
                }                    
            }

            for(int i = 0; i < _graph.Count; i++)
            {
                foreach(var to in _graph[i].Outgoing)
                {
                    dist[i][to] = 1;
                }
            }

            // Floyd–Warshall algorithm
            for (int k = 0; k < _graph.Count; k++)
            {
                for(int i = 0; i < _graph.Count; i++)
                {
                    for(int j = 0; j < _graph.Count; j++)
                    {
                        if (dist[i][j] > dist[i][k] + dist[k][j])
                            dist[i][j] = dist[i][k] + dist[k][j];
                    }
                }
            }

            var results = new List<List<bool>>();

            for (int i = 0; i < _graph.Count; i++)
            {
                results.Add(new List<bool>());               

                for (int x = 0; x < _graph.Count; x++)
                {
                    if (i == x)
                        results[i].Add(false);
                    else if (dist[i][x] < int.MaxValue / 2 - 1)
                        results[i].Add(true);
                    else
                        results[i].Add(false);
                }
                
            }

            return results;
        }
              
        protected class Node
        {
            public List<int> Outgoing;
            public bool Visited { get; set; }

            public Node()
            {
                Outgoing = new List<int>();
            }                       
        }
    }
}
