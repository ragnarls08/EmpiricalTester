using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
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
            var hkmstMoM = new DynamicGraph.HKMSTFinal(0.65, DynamicGraph.SPickMethod.MoM);
            //dynamicGraphs.Add(hkmstMoM);
            var hkmstMoMR = new DynamicGraph.HKMSTFinal(0.65, DynamicGraph.SPickMethod.MoMRandom);
            //dynamicGraphs.Add(hkmstMoMR);
            var hkmstR = new DynamicGraph.HKMSTFinal(0.65, DynamicGraph.SPickMethod.Random);
            //dynamicGraphs.Add(hkmstR);
            var hkmstQS = new DynamicGraph.HKMSTFinal(0.65, DynamicGraph.SPickMethod.QuickSelect);
            //dynamicGraphs.Add(hkmstQS);
            var hkmstDense = new HKMSTDense(100);
            dynamicGraphs.Add(hkmstDense);
            var bfgt = new BFGT();
            dynamicGraphs.Add(bfgt);
            

            var generator = new GraphGeneration.GraphGenerator();
            var ps = new List<double>() { 0.5  /*0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9*/ };
            for(var i = 1; i < 50000; i++)
            {
                foreach (var p in ps)
                {/*
                    generator.GenerateGraph(
                    6, // nodes
                    p, // Probability of an edge being added from a complete graph
                    i + "-Graph(15000, " + p + ")",
                    false, // writeToFile
                    true, // staticCheck
                    true, // topology compare
                    staticGraphs, dynamicGraphs);
                    //*/
                }
            }

            var ms = new List<int>() { 340000, 360000, 380000, 400000,
                                       420000, 440000, 460000, 480000, 500000,
                                       520000, 540000, 560000, 580000, 600000 };
            //generator.GenerateGraphs(5000, ms, 100, @"C:\Users\Ragnar\Dropbox\Chalmers\Thesis\Graphs\NotSparse", staticGraphs, dynamicGraphs );



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

            //runner.RunDirectoryPerGraph(@"C:\Users\Ragnar\Dropbox\Chalmers\Thesis\Graphs\SuperSparse", staticGraphs, dynamicGraphs);

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

            /*
            var dense = new HKMSTDense(3);
            for (int i = 0; i < 3; i++)
            {
                dense.AddVertex();
            }

            var b1 = dense.AddEdge(2, 1);
            var b2 = dense.AddEdge(2, 0);
            var b3 = dense.AddEdge(0, 1);
            var b4 = dense.AddEdge(1, 2);

            
            //*/

            
            bfgt = new BFGT();
            bfgt.ResetAll(6);
            for (int i = 0; i < 6; i++)
            {
                bfgt.AddVertex();
            }

            /*
    [0]: {(6, 1)}
    [1]: {(1, 3)}
    [2]: {(4, 3)}
    [3]: {(6, 0)}
    [4]: {(5, 1)}

    [5]: {(2, 0)}
    [6]: {(1, 0)}
    [7]: {(5, 0)}
    [8]: {(4, 5)}
    [9]: {(1, 5)}

    [10]: {(0, 1)}
    [11]: {(0, 2)}
    [12]: {(4, 2)}
    [13]: {(3, 0)}
    [14]: {(5, 4)}

    [15]: {(0, 5)}
    [16]: {(1, 2)}
    [17]: {(1, 4)}
    [18]: {(4, 6)}
    [19]: {(2, 5)}

    [20]: {(2, 4)}
    [21]: {(6, 4)}
    [22]: {(0, 3)}
    [23]: {(6, 3)}
    0 3
            */

            
            var b1 = bfgt.AddEdge(4, 1);
            var b2 = bfgt.AddEdge(5, 1);
            var b3 = bfgt.AddEdge(1, 3);
            var b4 = bfgt.AddEdge(3, 0);
            var b5 = bfgt.AddEdge(2, 0);

            var b6 = bfgt.AddEdge(1, 2);


            /*
            var b1 = bfgt.AddEdge(6, 1);
            var b2 = bfgt.AddEdge(1, 3);
            var b3 = bfgt.AddEdge(4, 3);
            var b4 = bfgt.AddEdge(6, 0);
            var b5 = bfgt.AddEdge(5, 1);

            var b6 = bfgt.AddEdge(2, 0);
            var b7 = bfgt.AddEdge(1, 0);
            var b8 = bfgt.AddEdge(5, 0);
            var b9 = bfgt.AddEdge(4, 5);
            var b10 = bfgt.AddEdge(1, 5);//

            var b11 = bfgt.AddEdge(0, 1);//
            var b12 = bfgt.AddEdge(0, 2);//
            var b13 = bfgt.AddEdge(4, 2);
            var b14 = bfgt.AddEdge(3, 0);
            var b15 = bfgt.AddEdge(5, 4);//

            var b16 = bfgt.AddEdge(0, 5);//
            var b17 = bfgt.AddEdge(1, 2);
            var b18 = bfgt.AddEdge(1, 4);//
            var b19 = bfgt.AddEdge(4, 6);
            var b20 = bfgt.AddEdge(2, 5);//

            var b21 = bfgt.AddEdge(2, 4);//
            var b22 = bfgt.AddEdge(6, 4);//
            var b23 = bfgt.AddEdge(0, 3);//
            var b24 = bfgt.AddEdge(6, 3);
            var b25 = bfgt.AddEdge(0, 3);// 
            //*/





             Console.WriteLine("\n\ndone");
            Console.ReadLine();            
        }
    }
}