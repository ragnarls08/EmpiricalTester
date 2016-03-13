using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

using System.Windows.Forms.DataVisualization;

namespace EmpiricalTester
{
    class Program
    {
        static void Main(string[] args)
        {
            List<StaticGraph.IStaticGraph> staticGraphs = new List<StaticGraph.IStaticGraph>();

            StaticGraph.IStaticGraph kahn = new StaticGraph.Kahn();
            //staticGraphs.Add(kahn);
            StaticGraph.IStaticGraph tarjan = new StaticGraph.Tarjan();
            staticGraphs.Add(tarjan);

            List<DynamicGraph.IDynamicGraph> dynamicGraphs = new List<DynamicGraph.IDynamicGraph>();

            DynamicGraph.IDynamicGraph simple = new DynamicGraph.SimpleIncremental();
            dynamicGraphs.Add(simple);

            GraphGeneration.GraphGenerator generator = new GraphGeneration.GraphGenerator();
            /*
            generator.generateGraph(
                300, // nodes
                0.85, // Probability of an edge being added from a complete graph
                true, // writeToFile
                true, // staticCheck
                false, // topology compare
                staticGraphs, dynamicGraphs);
            //*/

            string[] fileNames = new string[] {
                //Path.Combine(Environment.CurrentDirectory, @"Output\20160307-05-38-36(100, 0.025).txt"),
                //Path.Combine(Environment.CurrentDirectory, @"Output\20160307-05-39-20(100, 0.05).txt"),
                Path.Combine(Environment.CurrentDirectory, @"Output\20160308-19-48-12(300, 0.85).txt"),
               
                //Path.Combine(Environment.CurrentDirectory, @"Output\20160307-04-07(20, 0.5).txt"),


                //Path.Combine(Environment.CurrentDirectory, @"Output\20160303-03-59(1000, 0.5).txt"),
                //Path.Combine(Environment.CurrentDirectory, @"Output\20160303-04-00(1000, 0.9).txt"),

                //Path.Combine(Environment.CurrentDirectory, @"Output\20160303-03-59(10000, 0.5).txt"),
                //Path.Combine(Environment.CurrentDirectory, @"Output\20160303-04-00(10000, 0.9).txt"),
                };

            GraphRunner.GraphRunner runner = new GraphRunner.GraphRunner();

            //runner.runGraph(fileNames, 5, false, true, staticGraphs, dynamicGraphs);
            Stopwatch sw = new Stopwatch();

            


            DataStructures.SGTree<int> omp = new DataStructures.SGTree<int>(0.75);

            var root = omp.insertFirst(1);

            for (int i = 1; i < 10000; i++)
            {
                omp.insert(root, i);
            }
            
            
            var two = omp.insert(root, 2);
            var three = omp.insert(root, 3);
            var four = omp.insert(two, 4);
            var five = omp.insert(root, 5);
            var six = omp.insert(three, 6);
            var seven = omp.insert(two, 7);
            var eight = omp.insert(four, 8);
            //*/

            //var x = omp.inOrderLabels();

        }
    }
}