using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace EmpiricalTester.GraphGeneration
{
    class GraphGenerator
    {
        private string filename;
        private List<Tuple<int, int>> edges;
        private List<Tuple<int, int>> allEdges;
        private int n;
        private int e;
        private bool staticCheck;
        private bool writeToFile;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="p"></param>
        /// <param name="staticCheck">If true always runs static check, otherwise only for the completed graph</param>
        /// <param name="staticGraphs"></param>
        /// <param name="dynamicGraphs"></param>
        /// <returns></returns>
        public string generateGraph(int n, double p, bool writeToFile, bool staticCheck, bool compareTopologies, List<StaticGraph.IStaticGraph> staticGraphs, List<DynamicGraph.IDynamicGraph> dynamicGraphs)
        {
            this.staticCheck = staticCheck;
            this.writeToFile = writeToFile;
            DateTime datenow = DateTime.Now;
            filename = datenow.ToString("yyyyMMdd-HH-mm-ss") + string.Format("({0}, {1})", n.ToString(), p.ToString());
            allEdges = new List<Tuple<int, int>>();

            // create all possible edges
            for(int i = 0; i < n; i++)
            {
                for(int x = 0; x < n; x++)
                {
                    // Don't add self connecting nodes
                    if(i != x)
                    {
                        allEdges.Add(new Tuple<int, int>(i, x));
                    }
                }
            }

            edges = new List<Tuple<int, int>>();
            this.n = n;

            Random random = new Random(DateTime.Now.Millisecond);
            allEdges = allEdges.OrderBy(item => random.Next()).ToList();
            
            for (int i = 0; i < n; i++)
            {
                foreach(StaticGraph.IStaticGraph graph in staticGraphs)
                {
                    graph.addVertex();
                }

                foreach(DynamicGraph.IDynamicGraph graph in dynamicGraphs)
                {
                    graph.addVertex();
                }
            }

            bool[] staticResults = new bool[staticGraphs.Count];
            bool[] dynamicResults = new bool[dynamicGraphs.Count];

            for(int i = 0; i < allEdges.Count; i++)
            {
                //System.Diagnostics.Debug.WriteLine("Progress Generation: " + (double)i/(double)n*100 +"%");                
                if(i % (allEdges.Count / 20) == 0)
                    Console.WriteLine("Adding edges: " + Math.Ceiling((double)i / (double)allEdges.Count * 100) + "%");
                var currentEdge = allEdges[i];                
                if (random.Next(0,100)/100.0 < p)
                {
                    int v = currentEdge.Item1;
                    int w = currentEdge.Item2;
                                    
                    for(int x = 0; x < staticGraphs.Count; x++)
                    {
                        staticGraphs[x].addEdge(v, w);
                        
                        if(staticCheck)
                        {
                            // if null there is a cycle
                            if (null == staticGraphs[x].topoSort())
                                staticResults[x] = false; // cycle detected
                            else
                                staticResults[x] = true; // all is good
                        }
                    }

                    for(int x = 0; x < dynamicGraphs.Count; x++)
                    {
                        // dynamic graph returns false if cycle is detected
                        dynamicResults[x] = dynamicGraphs[x].addEdge(v, w);
                            
                    }

                    // if static check is false only dynamic results matter
                    bool allAgree = staticCheck ? 
                        (staticResults.All(item => item == true) && dynamicResults.All(item => item == true))
                        || (staticResults.All(item => item == false) && dynamicResults.All(item => item == false))
                        : 
                        dynamicResults.All(item => item == true) || dynamicResults.All(item => item == false);

                    // if all are false i.e. all failed since there was a cycle
                    bool cycle = staticCheck ?
                        staticResults.All(item => item == false) && dynamicResults.All(item => item == false)
                        :
                        dynamicResults.All(item => item == false);

                    // if all do not agree
                    if ( ! allAgree )
                    {
                        edges.Add(new Tuple<int, int>(v, w)); // add the error edge to the list
                        writeFile("Crash-NotAgree");
                        throw new Exception("The algorithms do not agree");
                    }
                    
                    // cycle, remove this edge
                    if(cycle)
                    {
                        foreach(StaticGraph.IStaticGraph graph in staticGraphs)
                        {
                            graph.removeEdge(v, w);
                        }
                    }
                    else
                    {
                        e++; //increase edge count;
                        edges.Add(new Tuple<int, int>(v, w));
                    }
                }
            }

            // if static check is false, run a static check at the end
            if(!staticCheck)
            {
                Console.WriteLine("Performing final static check");

                for (int x = 0; x < staticGraphs.Count; x++)
                {
                    // if null there is a cycle
                    if (null == staticGraphs[x].topoSort())
                        staticResults[x] = false; // cycle detected
                    else
                        staticResults[x] = true; // all is good                 
                }

                // if not all agree throw error
                if(!(staticResults.All(item => item == true) || staticResults.All(item => item == false)))
                {
                    throw new Exception("Final static check failed");
                }

            }
            
            if(compareTopologies)
                topologyCompare(n, staticGraphs, dynamicGraphs);
                       
            if(writeToFile)
            {
                return writeFile("");
            }
            else
            {
                return "";
            }                        
        }

        private string writeFile(string comment)
        {
            bool exists = Directory.Exists(Path.Combine(Environment.CurrentDirectory, @"Output"));

            if (!exists)
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, @"Output"));

            string filenameComplete = Path.Combine(Environment.CurrentDirectory, @"Output\" + filename + comment + ".txt");

            edges.Insert(0, new Tuple<int, int>(n, e)); // insert the vertex and edge counters in the start of the file
            File.WriteAllLines(filenameComplete, edges.ConvertAll(item => item.Item1.ToString() + " " + item.Item2.ToString()).ToArray());

            return filenameComplete;
        }

        // Compares the edges topological order
        private void topologyCompare(int n, List<StaticGraph.IStaticGraph> staticGraphs, List<DynamicGraph.IDynamicGraph> dynamicGraphs)
        {
            Console.WriteLine("Starting topology comparer");
            // Find connected pairs to compare
            ConnectivityGraph connectivity = new ConnectivityGraph();
            
            for (int i = 0; i < n; i++)
            {
                connectivity.addVertex();
            }

            foreach(Tuple<int, int> edge in edges)
            {
                connectivity.addEdge(edge.Item1, edge.Item2);
            }

            // matrix of node connectivity
            Console.WriteLine("Generating connectivity matrix");
            var matrix = connectivity.generateConnectivityMatrix();
            var topologies = new List<Tuple<string, DataStructures.SGTree<int>, List<DataStructures.SGTNode<int>>>>();
            Console.WriteLine("Connectivity matrix completed");

            foreach (StaticGraph.IStaticGraph algorithm in staticGraphs)
            {
                var sgt = new DataStructures.SGTree<int>(0.75);
                var top = algorithm.topoSort();
                var items = new List<DataStructures.SGTNode<int>>();

                var prev = sgt.insertFirst(top[0]);
                items.Add(prev);
                for (int i = 1; i < top.Count(); i++)
                {
                    prev = sgt.insertAfter(prev, top[i]);
                    items.Add(prev);
                }

                items.Sort(Comparer<DataStructures.SGTNode<int>>.Create((i, j) => i.Value.CompareTo(j.Value)));
                topologies.Add(new Tuple<string, DataStructures.SGTree<int>, List<DataStructures.SGTNode<int>>>
                                         (algorithm.GetType().ToString(), sgt, items));
            }

            foreach (DynamicGraph.IDynamicGraph algorithm in dynamicGraphs)
            {
                var sgt = new DataStructures.SGTree<int>(0.75);
                var top = algorithm.topology();
                var items = new List<DataStructures.SGTNode<int>>();

                var prev = sgt.insertFirst(top[0]);
                items.Add(prev);
                for (int i = 1; i < top.Count(); i++)
                {
                    prev = sgt.insertAfter(prev, top[i]);
                    items.Add(prev);
                }

                items.Sort(Comparer<DataStructures.SGTNode<int>>.Create((i, j) => i.Value.CompareTo(j.Value)));
                topologies.Add(new Tuple<string, DataStructures.SGTree<int>, List<DataStructures.SGTNode<int>>>(algorithm.GetType().ToString(), sgt, items));
            }

            Console.Write("Performing topology compare");
            for(int i = 0; i < matrix.Count; i++)
            {
                for(int j = 0; j < matrix.Count; j++)
                {
                    if (matrix[i][j])
                    { 
                        foreach (var algorithm in topologies)
                        {
                            if(!algorithm.Item2.query(algorithm.Item3[i], algorithm.Item3[j]))
                            {
                                writeFile("TopoCompFail");
                                throw new Exception("Topological comparison failed for: " + algorithm.Item1);
                            }
                        }                    
                    }
                }
            }
        }
    }
}
