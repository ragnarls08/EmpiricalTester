﻿using System;
using System.IO;
using System.Collections.Generic;
using EmpiricalTester.DynamicGraph;

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

            var dynamicGraphs = new List<IDynamicGraph>();
            
            var simple = new SimpleIncremental();
            dynamicGraphs.Add(simple);

            var hkmstV1 = new HKMSTV1(0.65);
            dynamicGraphs.Add(hkmstV1);
            var hkmstMoM = new HKMSTFinal(0.65, SPickMethod.MoM);
            dynamicGraphs.Add(hkmstMoM);
            var hkmstMoMR = new HKMSTFinal(0.65, SPickMethod.MoMRandom);
            dynamicGraphs.Add(hkmstMoMR);
            var hkmstR = new HKMSTFinal(0.65, SPickMethod.Random);
            dynamicGraphs.Add(hkmstR);
            var hkmstQS = new HKMSTFinal(0.65, SPickMethod.QuickSelect);
            dynamicGraphs.Add(hkmstQS);

            var hkmstDense = new HKMSTDense(100);
            dynamicGraphs.Add(hkmstDense);

            var bfgt = new BFGT {CycleBackup = true};
            dynamicGraphs.Add(bfgt);

            var bfgtDense = new BFGTDense() {CycleBackup = true};
            dynamicGraphs.Add(bfgtDense);

            var pk = new PearceKelly();
            dynamicGraphs.Add(pk);
            


            var generator = new GraphGeneration.GraphGenerator();
            var ps = new List<double>() { 0.5  /*0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9*/ };
            for(var i = 1; i < 10000; i++)
            {
                foreach (var p in ps)
                {
                    generator.GenerateGraph(
                    40, // nodes
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

           /*
            pk = new PearceKelly();
            for (int i = 0; i < 3; i++)
            {
                pk.AddVertex();
            }

               
            var b1 = pk.AddEdge(1, 0);
            var b2 = pk.AddEdge(2, 1);
            var b3 = pk.AddEdge(0, 1);
            var b4 = pk.AddEdge(0, 2);
            //var basdf = cfkr1.Topology();
            //var b4 = cfkr.AddEdge(0, 1);
            //var b5 = cfkr.AddEdge(0, 1);
            //*/
            Console.WriteLine("\n\ndone");
            Console.ReadLine();            
        }
    }
}