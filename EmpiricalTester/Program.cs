using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace EmpiricalTester
{
    class Program
    {
        static void Main(string[] args)
        {
            GraphGeneration.GraphGenerator generator = new GraphGeneration.GraphGenerator();

            generator.generateGraph(1000, 0.5);

            /*
            //string tiny = Path.Combine(Environment.CurrentDirectory, @"Inputs\tiny.txt");
            string tiny = Path.Combine(Environment.CurrentDirectory, @"Inputs\tiny_cycle.txt");

            string[] text = File.ReadAllLines(tiny);
            List<Tuple<int, int>> graph = new List<Tuple<int, int>>();                       

            for (int i = 0; i < text.Length; i++)
            {
                string[] s = text[i].Split(' ');
                graph.Add(new Tuple<int, int>(int.Parse(s[0]), int.Parse(s[1])));
            }

            int vertexCount = graph[0].Item1;
            int edgeCount = graph[0].Item2;

            graph.RemoveAt(0);

            StaticGraph.IStaticGraph kahn = new StaticGraph.Kahn();
            StaticGraph.IStaticGraph tarjan = new StaticGraph.Tarjan();

            for(int i = 0; i < vertexCount; i++)
            {
                kahn.addVertex();
                tarjan.addVertex();
            }

            foreach (Tuple<int, int> edge in graph)
            {
                kahn.addEdge(edge.Item1, edge.Item2);
                tarjan.addEdge(edge.Item1, edge.Item2);
            }

            int[] result = kahn.topoSort();
            int[] result2 = tarjan.topoSort();
            */
            
        }        
    }
}
