using System;
using System.IO;
using System.Collections.Generic;
using static System.Console;
using EmpiricalTester.Algorithms;
using EmpiricalTester.DataStructures;
using EmpiricalTester.DynamicGraph;
using static EmpiricalTester.Measuring.MedianMeasure;


namespace EmpiricalTester
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var staticGraphs = new List<StaticGraph.IStaticGraph>();

            var kahn = new StaticGraph.Kahn();
            staticGraphs.Add(kahn);
            var tarjan = new StaticGraph.Tarjan();
            staticGraphs.Add(tarjan);

            var dynamicGraphs = new List<DynamicGraph.IDynamicGraph>();

            var simple = new DynamicGraph.SimpleIncremental();
            dynamicGraphs.Add(simple);
            var hkmstV1 = new DynamicGraph.HKMSTV1(0.65);
            dynamicGraphs.Add(hkmstV1);
            var hkmst = new DynamicGraph.HKMSTFinal(0.65, DynamicGraph.SPickMethod.MoM);
            dynamicGraphs.Add(hkmst);
            //var hkmstDense = new HKMSTDense(100);
            //dynamicGraphs.Add(hkmstDense);
            var bfgt = new BFGT();
            dynamicGraphs.Add(bfgt);
            

            var generator = new GraphGeneration.GraphGenerator();
            var ps = new List<double>() { 0.9, /*0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9*/ };
            for(var i = 1; i < 50; i++)
            {
                foreach (var p in ps)
                {/*
                    generator.GenerateGraph(
                    5, // nodes
                    p, // Probability of an edge being added from a complete graph
                    i + "-Graph(5000, " + p + ")",
                    false, // writeToFile
                    false, // staticCheck
                    true, // topology compare
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


                @"C:\Users\Ragnar\Dropbox\Chalmers\Thesis\Graphs\2500\0.3\3-Graph(2500, 0.3).txt",
                //Path.Combine(Environment.CurrentDirectory, @"Output\20160317-07-25-48(1000, 0.65).txt"),
                //Path.Combine(Environment.CurrentDirectory, @"Output\20160317-07-25-18(1000, 0.75).txt"),
                //Path.Combine(Environment.CurrentDirectory, @"Output\20160317-07-24-33(1000, 0.85).txt"),                              
                //Path.Combine(Environment.CurrentDirectory, @"Output\20160317-07-23-52(1000, 0.95).txt"),

               

                //Path.Combine(Environment.CurrentDirectory, @"Output\20160303-04-00(1000, 0.9).txt"),

                //Path.Combine(Environment.CurrentDirectory, @"Output\20160303-03-59(10000, 0.5).txt"),
                //Path.Combine(Environment.CurrentDirectory, @"Output\20160303-04-00(10000, 0.9).txt"),
                };


            
            var runner = new Measuring.GraphRunner();
            //runner.runGraph(fileNames, 10, false, false, staticGraphs, dynamicGraphs);
            //runner.runStaticVsDynamic(fileNames, 5, true, false, staticGraphs, dynamicGraphs);
            /*
            runner.runFolder(
                @"C:\Users\Ragnar\Dropbox\Chalmers\Thesis\Graphs\1000",
                @"C:\Users\Ragnar\Dropbox\Chalmers\Thesis\Graphs\1000\outPut",
                5,
                5000,
                dynamicGraphs
                );
                //*/

            var measure = new Measuring.OrderMaintenance();
            string outFile = Path.Combine(Environment.CurrentDirectory, @"Output\ompMeasure3.txt");
            var ns = new List<int>() { 10 };
            ps = new List<double>() { 0.05, 0.15, 0.25, 0.35, 0.45, 0.55, 0.65, 0.75, 0.85, 0.95 };
            var alphas = new List<double>() { 0.6, 0.65, 0.7, 0.75 };
            //measure.run(outFile, ns, ps, alphas, 5);
            //measure.runSequence(outFile, ns, ps, alphas, 5);
            /*
            ns = new List<int>() { 1000, 10000, 100000, 1000000 };
            WriteLine(MeasureMedian(ns, 0, 100, 25)); */

            var blebb = new List<int>() { 1,2,3,4,5,6 };



            
            bfgt = new BFGT();
            for (int i = 0; i < 5; i++)
            {
                bfgt.AddVertex();
            }

            var b1 = bfgt.AddEdge(3, 1);
            var b2 = bfgt.AddEdge(0, 4);
            var b3 = bfgt.AddEdge(3, 0);

            var bf = bfgt.AddEdge(4, 3);
            var bc = bfgt.AddEdge(1, 3);
            var bd = bfgt.AddEdge(0, 3);
            var be = bfgt.AddEdge(4, 0);
            


            var b4 = bfgt.AddEdge(2, 4);
            var b5 = bfgt.AddEdge(1, 0);
            var b6 = bfgt.AddEdge(2, 1);
            var b7 = bfgt.AddEdge(4, 3);



            //*/
            
            



            Console.WriteLine("\n\ndone");
            Console.ReadLine();


            



        }
    }
}