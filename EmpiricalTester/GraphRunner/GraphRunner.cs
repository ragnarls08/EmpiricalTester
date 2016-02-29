using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace EmpiricalTester.GraphRunner
{
    class GraphRunner
    {
        public void runGraph(string fileName, List<StaticGraph.IStaticGraph> staticGraphs, List<DynamicGraph.IDynamicGraph> dynamicGraphs)
        {
            InputFile graph = readFile(fileName);
            Stopwatch sw = new Stopwatch();
            List<Measurements> measurements = new List<Measurements>();


            // Add all the nodes to the graphs
            // TODO is this ok or should adding nodes be a part of the measurements
            addNodes(graph.n, staticGraphs, dynamicGraphs);


            // TODO simpleincremental is quite fast already, maybe edges need to be added in "batches" for the measurments to be more accurate
            foreach (StaticGraph.IStaticGraph algorithm in staticGraphs)
            {
                measurements.Add(new Measurements(algorithm.GetType().ToString()));
                Measurements current = measurements.Find(item => item.Name == algorithm.GetType().ToString());

                foreach (Pair pair in graph.edges)
                {
                    sw.Start();
                    algorithm.addEdge(pair.from, pair.to);
                    algorithm.topoSort();
                    sw.Stop();
                    current.timeElapsed.Add(sw.Elapsed);                    
                    sw.Reset();
                }
            }

            foreach (DynamicGraph.IDynamicGraph algorithm in dynamicGraphs)
            {
                measurements.Add(new Measurements(algorithm.GetType().ToString()));
                Measurements current = measurements.Find(item => item.Name == algorithm.GetType().ToString());

                foreach (Pair pair in graph.edges)
                {
                    sw.Start();
                    algorithm.addEdge(pair.from, pair.to);
                    sw.Stop();
                    current.timeElapsed.Add(sw.Elapsed);
                    sw.Reset();
                }
            }

            measurements.ForEach(item => item.updateStatistics());
        }
        

        /// <summary>
        /// Initializes the graphs with nodes. 
        /// </summary>
        /// <param name="n">Number of nodes</param>
        /// <param name="staticGraphs"></param>
        /// <param name="dynamicGraphs"></param>
        private void addNodes(int n, List<StaticGraph.IStaticGraph> staticGraphs, List<DynamicGraph.IDynamicGraph> dynamicGraphs)
        {
            for(int i = 0; i < n; i++)
            {
                foreach(StaticGraph.IStaticGraph algorithm in staticGraphs)
                {
                    algorithm.addVertex();
                }

                foreach(DynamicGraph.IDynamicGraph algorithm in dynamicGraphs)
                {
                    algorithm.addVertex();
                }
            }
        }

        private InputFile readFile(string fileName)
        {
            string[] text = File.ReadAllLines(fileName);
            InputFile retValue = new InputFile();
            retValue.n = int.Parse(text[0].Split().First());
            retValue.m = int.Parse(text[0].Split().Skip(1).First());


            // split input and add to the graph list
            for (int i = 1; i < text.Length; i++)
            {
                string[] s = text[i].Split();
                retValue.edges.Add(new Pair(int.Parse(s.First()), int.Parse(s.Skip(1).First())));
            }
            return retValue;
        }

        // data object to keep elapsed times for each algorithm
        private class Measurements
        {
            public string Name { get; set; }
            public List<TimeSpan> timeElapsed { get; set; }

            public double averageTick { get; set; }
            public double maxTick { get; set; }
            public double minTick { get; set; }

            public Measurements(string name)
            {
                this.Name = name;
                timeElapsed = new List<TimeSpan>();
            }

            public void updateStatistics()
            {
                averageTick = timeElapsed.Average(item => item.Ticks);
                maxTick = timeElapsed.Max(item => item.Ticks);
                minTick = timeElapsed.Min(item => item.Ticks);
            }
        }

        // data object for file contents
        private class InputFile
        {
            public int n { get; set; } // #nodes
            public int m { get; set; } // #edges

            public List<Pair> edges { get; set; }

            public InputFile()
            {
                edges = new List<Pair>();
            }
        }

        private struct Pair
        {
            public int from { get; set; }
            public int to { get; set; }

            public Pair(int from, int to)
            {
                this.from = from;
                this.to = to;
            }
        }
    }
}
