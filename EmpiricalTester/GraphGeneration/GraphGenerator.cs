using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using EmpiricalTester.DynamicGraph;
using EmpiricalTester.StaticGraph;


namespace EmpiricalTester.GraphGeneration
{
    class GraphGenerator
    {
        private string _filename;
        private List<Tuple<int, int>> _edges;
        private List<Tuple<int, int>> _allEdges;
        private int _n;
        private int _e;

        /// <summary>
        /// Generates a random graph 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="p">Probability of an edge being added</param>
        /// <param name="writeToFile">If true writes the graph to a file</param>
        /// <param name="staticCheck">If true always runs static check, otherwise only for the completed graph</param>
        /// <param name="compareTopologies">Compares the topologies of the algorithms</param>
        /// <param name="staticGraphs">Static graph algorithms to use for cycle checks</param>
        /// <param name="dynamicGraphs">Dynamic graph algorithms to use for cycle checks</param>
        /// <param name="filename">Full filename of the output file</param>
        /// <returns>Filename</returns>
        public string GenerateGraph(int n, double p, string filename, bool writeToFile, bool staticCheck, bool compareTopologies, List<IStaticGraph> staticGraphs, List<IDynamicGraph> dynamicGraphs)
        {
            _n = n;
            _e = 0;
            _filename = filename;
            _allEdges = new List<Tuple<int, int>>(_n);

            staticGraphs.ForEach(item => item.ResetAll());
            dynamicGraphs.ForEach(item => item.ResetAll());
            var random = new Random(DateTime.Now.Millisecond);

            // create all possible edges
            for (var i = 0; i < _n; i++)
            {
                for(var x = 0; x < _n; x++)
                {
                    // Don't add self connecting nodes
                    if(i != x)
                    {
                        _allEdges.Add(new Tuple<int, int>(i, x));
                    }
                }
            }

            _edges = new List<Tuple<int, int>>();
            _allEdges = _allEdges.OrderBy(item => random.Next()).ToList();
            
            for (var i = 0; i < _n; i++)
            {
                foreach(var graph in staticGraphs)
                {
                    graph.AddVertex();
                }

                foreach(var graph in dynamicGraphs)
                {
                    graph.AddVertex();
                }
            }

            var staticResults = new bool[staticGraphs.Count];
            var dynamicResults = new bool[dynamicGraphs.Count];

            for(var i = 0; i < _allEdges.Count; i++)
            {
                if(i % (_allEdges.Count / 20) == 0)
                    Console.WriteLine("Adding edges: " + Math.Ceiling((double)i / (double)_allEdges.Count * 100) + "%");
                
                if (!(random.Next(0, 100)/100.0 < p))
                    continue;

                var currentEdge = _allEdges[i];
                var v = currentEdge.Item1;
                var w = currentEdge.Item2;
                                         
                for(var x = 0; x < staticGraphs.Count; x++)
                {
                    staticGraphs[x].AddEdge(v, w);
                        
                    if(staticCheck)
                    {
                        // if null there is a cycle
                        if (null == staticGraphs[x].TopoSort())
                            staticResults[x] = false; // cycle detected
                        else
                            staticResults[x] = true; // all is good
                    }
                }

                for(var x = 0; x < dynamicGraphs.Count; x++)
                {
                    // dynamic graph returns false if cycle is detected
                    dynamicResults[x] = dynamicGraphs[x].AddEdge(v, w);
                            
                }

                // if static check is false only dynamic results matter
                var allAgree = staticCheck ? 
                    (staticResults.All(item => item) && dynamicResults.All(item => item))
                    || (staticResults.All(item => item == false) && dynamicResults.All(item => item == false))
                    : 
                    dynamicResults.All(item => item) || dynamicResults.All(item => item == false);

                // if all are false i.e. all failed since there was a cycle
                var cycle = staticCheck ?
                    staticResults.All(item => item == false) && dynamicResults.All(item => item == false)
                    :
                    dynamicResults.All(item => item == false);

                // if all do not agree
                if ( ! allAgree )
                {
                    _edges.Add(new Tuple<int, int>(v, w)); // add the error edge to the list
                    WriteFile("Crash-NotAgree");

                    throw new Exception("The algorithms do not agree");
                }
                    
                // cycle, remove this edge
                if(cycle)
                {
                    foreach(var graph in staticGraphs)
                    {
                        graph.RemoveEdge(v, w);
                    }
                }
                else
                {
                    _e++; //increase edge count;
                    _edges.Add(new Tuple<int, int>(v, w));
                }
            }

            // if static check is false, run a static check at the end
            if(!staticCheck)
            {
                Console.WriteLine("Performing final static check");

                for (int x = 0; x < staticGraphs.Count; x++)
                {
                    // if null there is a cycle
                    if (null == staticGraphs[x].TopoSort())
                        staticResults[x] = false; // cycle detected
                    else
                        staticResults[x] = true; // all is good                 
                }

                // if not all agree throw error
                if(!(staticResults.All(item => item) || staticResults.All(item => item == false)))
                {
                    throw new Exception("Final static check failed");
                }

            }
            
            if(compareTopologies)
                TopologyCompare(staticGraphs, dynamicGraphs);
                       
            if(writeToFile)
            {
                return WriteFile("");
            }
            else
            {
                return "";
            }                        
        }

        private string WriteFile(string comment)
        {
            var exists = Directory.Exists(Path.Combine(Environment.CurrentDirectory, @"Output"));

            if (!exists)
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, @"Output"));

            var filenameComplete = Path.Combine(Environment.CurrentDirectory, @"Output\" + _filename + comment + ".txt");

            _edges.Insert(0, new Tuple<int, int>(_n, _e)); // insert the vertex and edge counters in the start of the file
            File.WriteAllLines(filenameComplete, _edges.ConvertAll(item => item.Item1.ToString() + " " + item.Item2.ToString()).ToArray());

            return filenameComplete;
        }

        // Compares the edges topological order
        private void TopologyCompare(IEnumerable<IStaticGraph> staticGraphs, IEnumerable<IDynamicGraph> dynamicGraphs)
        {
            Console.WriteLine("Starting topology comparer");
            // Find connected pairs to compare
            ConnectivityGraph connectivity = new ConnectivityGraph();
            
            for (int i = 0; i < _n; i++)
            {
                connectivity.AddVertex();
            }

            foreach(Tuple<int, int> edge in _edges)
            {
                connectivity.AddEdge(edge.Item1, edge.Item2);
            }

            // matrix of node connectivity
            Console.WriteLine("Generating connectivity matrix");
            var matrix = connectivity.GenerateConnectivityMatrix();
            var topologies = new List<Tuple<string, DataStructures.SGTree<int>, List<DataStructures.SGTNode<int>>>>();
            Console.WriteLine("Connectivity matrix completed");

            foreach (var algorithm in staticGraphs)
            {
                var sgt = new DataStructures.SGTree<int>(0.75);
                var top = algorithm.TopoSort();
                var items = new List<DataStructures.SGTNode<int>>();

                var prev = sgt.insertFirst(top[0]);
                items.Add(prev);
                for (var i = 1; i < top.Length; i++)
                {
                    prev = sgt.insertAfter(prev, top[i]);
                    items.Add(prev);
                }

                items.Sort(Comparer<DataStructures.SGTNode<int>>.Create((i, j) => i.Value.CompareTo(j.Value)));
                topologies.Add(new Tuple<string, DataStructures.SGTree<int>, List<DataStructures.SGTNode<int>>>
                                         (algorithm.GetType().ToString(), sgt, items));
            }

            foreach (var algorithm in dynamicGraphs)
            {
                var sgt = new DataStructures.SGTree<int>(0.75);
                var top = algorithm.Topology();
                var items = new List<DataStructures.SGTNode<int>>();

                var prev = sgt.insertFirst(top[0]);
                items.Add(prev);
                for (var i = 1; i < top.Count; i++)
                {
                    prev = sgt.insertAfter(prev, top[i]);
                    items.Add(prev);
                }

                items.Sort(Comparer<DataStructures.SGTNode<int>>.Create((i, j) => i.Value.CompareTo(j.Value)));
                topologies.Add(new Tuple<string, DataStructures.SGTree<int>, List<DataStructures.SGTNode<int>>>(algorithm.GetType().ToString(), sgt, items));
            }

            Console.Write("Performing topology compare");
            for(var i = 0; i < matrix.Count; i++)
            {
                for(var j = 0; j < matrix.Count; j++)
                {
                    if (matrix[i][j])
                    { 
                        foreach (var algorithm in topologies)
                        {
                            //if(!algorithm.Item2.query(algorithm.Item3[i], algorithm.Item3[j]))
                            if (!algorithm.Item2.Query(algorithm.Item3[j], algorithm.Item3[i]))
                            {
                                WriteFile("TopoCompFail");
                                throw new Exception("Topological comparison failed for: " + algorithm.Item1);
                            }
                        }                    
                    }
                }
            }
        }
    }
}
