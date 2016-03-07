using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;

namespace EmpiricalTester.GraphRunner
{
    class GraphRunner
    {
        public void runGraph(string[] fileNames, int repeateCount, bool writeToFile, bool makeGraphImage, List<StaticGraph.IStaticGraph> staticGraphs, List<DynamicGraph.IDynamicGraph> dynamicGraphs)
        {
            for(int x = 0; x < fileNames.Length; x++)
            {
                string fileName = fileNames[x];
                InputFile graph = readFile(fileName);
                Stopwatch sw = new Stopwatch();
                List<Measurements> measurements = new List<Measurements>();

                Console.WriteLine("File " + (x+1) + "/ " + fileNames.Length);

                // TODO simpleincremental is quite fast already, maybe edges need to be added in "batches" for the measurments to be more accurate
                foreach (StaticGraph.IStaticGraph algorithm in staticGraphs)
                {
                    measurements.Add(new Measurements(algorithm.GetType().ToString()));
                    Measurements current = measurements.Find(item => item.Name == algorithm.GetType().ToString());
                    
                    for (int i = 0; i < repeateCount; i++)
                    {
                        Console.WriteLine("Run: " + i + ", " + current.Name);
                        algorithm.resetAll();
                        // add nodes
                        for (int y = 0; y < graph.n; y++)
                        {
                            algorithm.addVertex();
                        }

                        current.timeElapsed.Add(new List<TimeSpan>());
                        foreach (Pair pair in graph.edges)
                        {
                            sw.Start();
                            algorithm.addEdge(pair.from, pair.to);
                            algorithm.topoSort();
                            sw.Stop();
                            current.timeElapsed[i].Add(sw.Elapsed);
                            sw.Reset();
                        }
                    }

                }

                foreach (DynamicGraph.IDynamicGraph algorithm in dynamicGraphs)
                {
                    measurements.Add(new Measurements(algorithm.GetType().ToString()));
                    Measurements current = measurements.Find(item => item.Name == algorithm.GetType().ToString());
                    
                    for (int i = 0; i < repeateCount; i++)
                    {
                        Console.WriteLine("Run: " + i + ", " + current.Name);
                        algorithm.resetAll();
                        // add nodes
                        for (int y = 0; y < graph.n; y++)
                        {
                            algorithm.addVertex();
                        }

                        current.timeElapsed.Add(new List<TimeSpan>());
                        foreach (Pair pair in graph.edges)
                        {
                            System.Diagnostics.Debug.WriteLine(pair.from + " - " + pair.to);
                            sw.Start();
                            algorithm.addEdge(pair.from, pair.to);
                            sw.Stop();
                            current.timeElapsed[i].Add(sw.Elapsed);
                            sw.Reset();
                        }
                    }
                }

                measurements.ForEach(item => item.updateStatistics());

                if(writeToFile)
                    writeMeasurements(fileName, measurements);
                if (makeGraphImage)
                    createGraph(fileName, measurements, graph.n, graph.edges.Count, repeateCount);
            }            
        }

        private void createGraph(string fileName, List<Measurements> measurements, int n, int m, int repeateCount)
        {
            Chart chart = new Chart();
            chart.ChartAreas.Add("area");
            chart.ChartAreas["area"].AxisX.Name = "Nr of edge";
            chart.ChartAreas["area"].AxisY.Name = "Ticks";
            chart.ChartAreas["area"].AxisY.Maximum = 15000; // TODO this needs to be in relation to the data
            // controls the resolution of the output file            
            chart.Width = 1920;
            chart.Height = 1080;

            chart.Titles.Add(string.Format("{0} nodes, {1} edges. Average of {2} runs", n, m, repeateCount));
            
            chart.Palette = ChartColorPalette.SeaGreen;

            
            foreach (var algorithm in measurements)
            {
                string algorithmName = algorithm.Name.Substring(algorithm.Name.LastIndexOf('.') + 1);

                chart.Series.Add(algorithmName);
                chart.Series[algorithmName].ChartType = SeriesChartType.Line;
                chart.Legends.Add(algorithmName);

                foreach (long tick in algorithm.averages)
                {
                    chart.Series[algorithmName].Points.AddY(tick);
                }                              
            }
            
            chart.SaveImage(fileName + ".png", ChartImageFormat.Png);
        }

        private void writeMeasurements(string fileName, List<Measurements> measurements)
        {
            fileName = fileName + ".measurements";

            List<string> lines = new List<string>();
            
            foreach(var algorithm in measurements)
            {   
                lines.Add(algorithm.Name + ";" + algorithm.averages.ConvertAll<string>(item => item.ToString()).Aggregate((a, b) => a + ";" + b));
            }

            File.WriteAllLines(fileName, lines);
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
            public List<List<TimeSpan>> timeElapsed { get; set; }
            public List<long> averages { get; set; } // long is the datatype of tick

            public double averageTick { get; set; }
            public double maxTick { get; set; }
            public double minTick { get; set; }

            public Measurements(string name)
            {
                this.Name = name;
                timeElapsed = new List<List<TimeSpan>>();
                averages = new List<long>();
            }

            public void updateStatistics()
            {
                for(int edge = 0; edge < timeElapsed[0].Count; edge++)
                {
                    long sum = 0;
                    for(int runNumber = 0; runNumber < timeElapsed.Count; runNumber++)
                    {
                        sum += timeElapsed[runNumber][edge].Ticks;
                    }

                    averages.Add(sum / timeElapsed.Count);
                }

                averageTick = averages.Average();
                maxTick = averages.Max();
                minTick = averages.Min();
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
