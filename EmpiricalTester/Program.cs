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
            staticGraphs.Add(kahn);
            StaticGraph.IStaticGraph tarjan = new StaticGraph.Tarjan();
            staticGraphs.Add(tarjan);

            List<DynamicGraph.IDynamicGraph> dynamicGraphs = new List<DynamicGraph.IDynamicGraph>();

            DynamicGraph.IDynamicGraph simple = new DynamicGraph.SimpleIncremental();
            dynamicGraphs.Add(simple);

            GraphGeneration.GraphGenerator generator = new GraphGeneration.GraphGenerator();
            /*
            generator.generateGraph(
                100, // nodes
                0.075, // Probability of an edge being added from a complete graph
                true, // writeToFile
                true, // staticCheck
                false, // topology compare
                staticGraphs, dynamicGraphs);
            //*/

            string[] fileNames = new string[] {
                Path.Combine(Environment.CurrentDirectory, @"Output\20160307-05-38-36(100, 0.025).txt"),
                Path.Combine(Environment.CurrentDirectory, @"Output\20160307-05-39-20(100, 0.05).txt"),
                Path.Combine(Environment.CurrentDirectory, @"Output\20160307-05-39-30(100, 0.075).txt"),
               
                //Path.Combine(Environment.CurrentDirectory, @"Output\20160307-04-07(20, 0.5).txt"),


                //Path.Combine(Environment.CurrentDirectory, @"Output\20160303-03-59(1000, 0.5).txt"),
                //Path.Combine(Environment.CurrentDirectory, @"Output\20160303-04-00(1000, 0.9).txt"),

                //Path.Combine(Environment.CurrentDirectory, @"Output\20160303-03-59(10000, 0.5).txt"),
                //Path.Combine(Environment.CurrentDirectory, @"Output\20160303-04-00(10000, 0.9).txt"),
                };

            GraphRunner.GraphRunner runner = new GraphRunner.GraphRunner();

            runner.runGraph(fileNames, 3, false, true, staticGraphs, dynamicGraphs);
            Stopwatch sw = new Stopwatch();


            /*
            DataStructures.SGTree<int> test = new DataStructures.SGTree<int>();
            SortedSet<int> test2 = new SortedSet<int>();

            int nr = 1000000;

            sw.Start();
            for(int i = 0; i<nr; i++)
            {
                test.Add(i);
            }                       
            sw.Stop();

            long sgtree = sw.ElapsedTicks;

            sw.Reset();

            sw.Start();
            for(int i = 0; i < nr; i++)
            {
                test2.Add(i);
            }
            sw.Stop();

            long csharp = sw.ElapsedTicks;

            */

            /*
            DataStructures.OMPList list = new DataStructures.OMPList(1.3);

            List<DataStructures.OMPNode> nodes = new List<DataStructures.OMPNode>();

            DataStructures.OMPNode first = list.InsertFirst(0);
            for(int i = 1; i < 9; i ++)
            {
                nodes.Add(list.Insert(first, i));
            }

            nodes.Add(list.Insert(first, 75));
            */
            //DataStructures.OMPNode ten = list.InsertFirst(10);
            //DataStructures.OMPNode nine = list.Insert(ten, 9);
            //DataStructures.OMPNode twenty = list.Insert(ten, 20);

            //einfaldara að láta omp bara sjá um nóðurnar, hinn algorithminn getur bara geymt ompnodes hja sér
            //noður algorithmans geta þa bara erft frá omp node, abstract eða eh...


        }        
    }
}
