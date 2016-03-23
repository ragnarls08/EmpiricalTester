using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
            //staticGraphs.Add(tarjan);

            List<DynamicGraph.IDynamicGraph> dynamicGraphs = new List<DynamicGraph.IDynamicGraph>();

            DynamicGraph.IDynamicGraph simple = new DynamicGraph.SimpleIncremental();
            dynamicGraphs.Add(simple);
            DynamicGraph.HKMST_V1 hkmst_v1 = new DynamicGraph.HKMST_V1(0.65);
            dynamicGraphs.Add(hkmst_v1);

            GraphGeneration.GraphGenerator generator = new GraphGeneration.GraphGenerator();
            var ps = new List<double>() { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9 };
            for(int i = 1; i < 11; i++)
            {
                foreach (var p in ps)
                {/*
                    generator.generateGraph(
                    250, // nodes
                    p, // Probability of an edge being added from a complete graph
                    i + "-Graph(250, " + p + ")",
                    true, // writeToFile
                    false, // staticCheck
                    false, // topology compare
                    staticGraphs, dynamicGraphs);
                    //*/
                }
            }
            


            string[] fileNames = new string[] {
                //Path.Combine(Environment.CurrentDirectory, @"Output\20160317-07-13-40(250, 0.55).txt"),
                //Path.Combine(Environment.CurrentDirectory, @"Output\20160317-07-13-36(250, 0.65).txt"),
                //Path.Combine(Environment.CurrentDirectory, @"Output\20160317-07-13-25(250, 0.75).txt"),
                //Path.Combine(Environment.CurrentDirectory, @"Output\20160317-07-13-15(250, 0.85).txt"),
                //Path.Combine(Environment.CurrentDirectory, @"Output\20160317-07-13-44(250, 0.95).txt"),


                //Path.Combine(Environment.CurrentDirectory, @"Output\20160317-07-26-18(1000, 0.55).txt"),
                //Path.Combine(Environment.CurrentDirectory, @"Output\20160317-07-25-48(1000, 0.65).txt"),
                //Path.Combine(Environment.CurrentDirectory, @"Output\20160317-07-25-18(1000, 0.75).txt"),
                //Path.Combine(Environment.CurrentDirectory, @"Output\20160317-07-24-33(1000, 0.85).txt"),                              
                //Path.Combine(Environment.CurrentDirectory, @"Output\20160317-07-23-52(1000, 0.95).txt"),

                Path.Combine(Environment.CurrentDirectory, @"Output\20160323-12-35-23(250, 0.1).txt"),
                Path.Combine(Environment.CurrentDirectory, @"Output\20160323-12-35-23(250, 0.2).txt"),
                Path.Combine(Environment.CurrentDirectory, @"Output\20160323-12-35-24(250, 0.3).txt"),
                Path.Combine(Environment.CurrentDirectory, @"Output\20160323-12-35-25(250, 0.4).txt"),
                Path.Combine(Environment.CurrentDirectory, @"Output\20160323-12-35-26(250, 0.5).txt"),

                Path.Combine(Environment.CurrentDirectory, @"Output\20160323-12-35-27(250, 0.6).txt"),
                Path.Combine(Environment.CurrentDirectory, @"Output\20160323-12-35-29(250, 0.7).txt"),
                Path.Combine(Environment.CurrentDirectory, @"Output\20160323-12-35-31(250, 0.8).txt"),
                Path.Combine(Environment.CurrentDirectory, @"Output\20160323-12-35-33(250, 0.9).txt"),

                //Path.Combine(Environment.CurrentDirectory, @"Output\20160303-04-00(1000, 0.9).txt"),

                //Path.Combine(Environment.CurrentDirectory, @"Output\20160303-03-59(10000, 0.5).txt"),
                //Path.Combine(Environment.CurrentDirectory, @"Output\20160303-04-00(10000, 0.9).txt"),
                };

            var runner = new Measuring.GraphRunner();
            //runner.runGraph(fileNames, 5, false, true, staticGraphs, dynamicGraphs);
            //runner.runStaticVsDynamic(fileNames, 5, true, false, staticGraphs, dynamicGraphs);
            runner.runFolder(
                @"C:\Users\Ragnar\Dropbox\Chalmers\Thesis\Graphs\250",
                @"C:\Users\Ragnar\Dropbox\Chalmers\Thesis\Graphs\250\outPut",
                5,
                staticGraphs
                , dynamicGraphs
                );

            
            var measure = new Measuring.OrderMaintenance();
            string outFile = Path.Combine(Environment.CurrentDirectory, @"Output\ompMeasure3.txt");
            var ns = new List<int>() { 10 };
            ps = new List<double>() { 0.05, 0.15, 0.25, 0.35, 0.45, 0.55, 0.65, 0.75, 0.85, 0.95 };
            var alphas = new List<double>() { 0.6, 0.65, 0.7, 0.75 };
            //measure.run(outFile, ns, ps, alphas, 5);
            //measure.runSequence(outFile, ns, ps, alphas, 5);


            Console.WriteLine("\n\ndone");
            Console.ReadLine();

        }
    }
}