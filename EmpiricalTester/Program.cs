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
            //generator.generateGraph(10000, 0.9, true, false, staticGraphs, dynamicGraphs);

            string[] fileNames = new string[] {
                Path.Combine(Environment.CurrentDirectory, @"Output\20160303-03-59(100, 0.5).txt"),
                Path.Combine(Environment.CurrentDirectory, @"Output\20160303-04-00(100, 0.9).txt"),


                Path.Combine(Environment.CurrentDirectory, @"Output\20160303-03-59(1000, 0.5).txt"),
                Path.Combine(Environment.CurrentDirectory, @"Output\20160303-04-00(1000, 0.9).txt"),

                Path.Combine(Environment.CurrentDirectory, @"Output\20160303-03-59(10000, 0.5).txt"),
                Path.Combine(Environment.CurrentDirectory, @"Output\20160303-04-00(10000, 0.9).txt"),
                };

            GraphRunner.GraphRunner runner = new GraphRunner.GraphRunner();
            
            runner.runGraph(fileNames, 5, staticGraphs, dynamicGraphs);


            /*
            DataStructures.OMPList list = new DataStructures.OMPList(1.3);

            List<DataStructures.OMPNode> nodes = new List<DataStructures.OMPNode>();

            DataStructures.OMPNode first = list.InsertFirst(0);
            for(int i = 1; i < 75; i ++)
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
