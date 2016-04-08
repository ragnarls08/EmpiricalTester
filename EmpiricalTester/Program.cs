using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using EmpiricalTester.Algorithms;

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

            

            var generator = new GraphGeneration.GraphGenerator();
            var ps = new List<double>() { 0.1, 0.2, 0.3, /*0.4, 0.5, 0.6, 0.7, 0.8, 0.9*/ };
            for(var i = 1; i < 5; i++)
            {
                foreach (var p in ps)
                {/*
                    generator.GenerateGraph(
                    100, // nodes
                    p, // Probability of an edge being added from a complete graph
                    i + "-Graph(5000, " + p + ")",
                    false, // writeToFile
                    true, // staticCheck
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
                15,
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
            Random r = new Random();
            List<int> x = new List<int>(120);
            for(int i = 0; i < 11; i++)
            {
                x.Add(r.Next(0, 200));
            }//*/

            var x = new List<int>() { 23, 15, 41, 34, 48, 42, 56, 69, 98, 99 };
            /*
            54
            147
            0
            */
            //var blee = x.Mom(x.Count / 2 + 1, Comparer<int>.Default);
            
            int median0 = Algorithms.Median.Mom(x, 0, Comparer<int>.Default);
            int median1 = Algorithms.Median.Mom(x, 1, Comparer<int>.Default);
            int median2 = Algorithms.Median.Mom(x, 2, Comparer<int>.Default);
            int median3 = Algorithms.Median.Mom(x, 3, Comparer<int>.Default);
            int median4 = Algorithms.Median.Mom(x, 4, Comparer<int>.Default);
            int median5 = Algorithms.Median.Mom(x, 5, Comparer<int>.Default);
            int median6 = Algorithms.Median.Mom(x, 6, Comparer<int>.Default);
            int median7 = Algorithms.Median.Mom(x, 7, Comparer<int>.Default);
            int median8 = Algorithms.Median.Mom(x, 8, Comparer<int>.Default);
            int median9 = Algorithms.Median.Mom(x, 9, Comparer<int>.Default);
            //int median10 = Algorithms.Median.Mom(x, 10, Comparer<int>.Default);
            //int median11 = Algorithms.Median.Mom(x, 11, Comparer<int>.Default);

            int b = Algorithms.Median.QuickSelect(x, x.Count / 2);
            var xl = new List<int>(x);
            xl.Sort();
            int c = xl.ElementAt(xl.Count / 2);
            //*/

            Console.WriteLine("\n\ndone");
            Console.ReadLine();

        }
    }
}