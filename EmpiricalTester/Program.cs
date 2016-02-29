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
            List<StaticGraph.IStaticGraph> staticGraphs = new List<StaticGraph.IStaticGraph>();

            StaticGraph.IStaticGraph kahn = new StaticGraph.Kahn();
            staticGraphs.Add(kahn);
            StaticGraph.IStaticGraph tarjan = new StaticGraph.Tarjan();
            staticGraphs.Add(tarjan);

            List<DynamicGraph.IDynamicGraph> dynamicGraphs = new List<DynamicGraph.IDynamicGraph>();

            DynamicGraph.IDynamicGraph simple = new DynamicGraph.SimpleIncremental();
            dynamicGraphs.Add(simple);

            //GraphGeneration.GraphGenerator generator = new GraphGeneration.GraphGenerator();
            //generator.generateGraph(100000, 0.9, true, false, staticGraphs, dynamicGraphs);

            
            GraphRunner.GraphRunner runner = new GraphRunner.GraphRunner();
            string tiny = Path.Combine(Environment.CurrentDirectory, @"Output\20160228-13-02(10000, 0.9).txt");
            runner.runGraph(tiny, staticGraphs, dynamicGraphs);
            
          

            /*
            //string tiny = Path.Combine(Environment.CurrentDirectory, @"Inputs\tiny.txt");
            string tiny = Path.Combine(Environment.CurrentDirectory, @"Inputs\tiny_cycle.txt");

            string[] text = File.ReadAllLines(tiny);
            List<Tuple<int, int>> graph = new List<Tuple<int, int>>();                       

            // split input and add to the graph list
            for (int i = 0; i < text.Length; i++)
            {
                string[] s = text[i].Split(' ');
                graph.Add(new Tuple<int, int>(int.Parse(s[0]), int.Parse(s[1])));
            }

            // remove the first number couple representing the counts
            int vertexCount = graph[0].Item1;
            int edgeCount = graph[0].Item2;

            graph.RemoveAt(0);



            //StaticGraph.IStaticGraph kahn = new StaticGraph.Kahn();
            //StaticGraph.IStaticGraph tarjan = new StaticGraph.Tarjan();
            DynamicGraph.IDynamicGraph simpleGraph = new DynamicGraph.SimpleIncremental();

            for(int i = 0; i < vertexCount; i++)
            {
                //kahn.addVertex();
                //tarjan.addVertex();
                simpleGraph.addVertex();
            }

            foreach (Tuple<int, int> edge in graph)
            {
                //kahn.addEdge(edge.Item1, edge.Item2);
                //tarjan.addEdge(edge.Item1, edge.Item2);
                if(!simpleGraph.addEdge(edge.Item1, edge.Item2))
                {
                    throw new Exception("CYCLE");
                }
            }

            //int[] result = kahn.topoSort();
            //int[] result2 = tarjan.topoSort();
            */
            
        }        
    }
}
