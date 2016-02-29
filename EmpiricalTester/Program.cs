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

            GraphGeneration.GraphGenerator generator = new GraphGeneration.GraphGenerator();
            generator.generateGraph(100, 0.9, true, false, staticGraphs, dynamicGraphs);

            /*
            GraphRunner.GraphRunner runner = new GraphRunner.GraphRunner();
            string tiny = Path.Combine(Environment.CurrentDirectory, @"Inputs\simpleIncrementalBug.txt");
            runner.runGraph(tiny, staticGraphs, dynamicGraphs);
            */
          

        }        
    }
}
