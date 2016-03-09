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

            foreach(Tuple<int, int> currentEdge in allEdges)
            {
                //System.Diagnostics.Debug.WriteLine("Progress Generation: " + (double)i/(double)n*100 +"%");
                System.Diagnostics.Debug.WriteLine(currentEdge.Item1);
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
            {
                //compare topologies
                bool compare = topologyCompare(n, staticGraphs, dynamicGraphs);

                if (!compare)
                {
                    writeFile("Crash-TopoOrder");
                    throw new Exception("Topological order comparison failed");
                }
            }
            

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
        private bool topologyCompare(int n, List<StaticGraph.IStaticGraph> staticGraphs, List<DynamicGraph.IDynamicGraph> dynamicGraphs)
        {
            System.Diagnostics.Debug.WriteLine("Starting topology comparer");
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
            List<Tuple<int, List<int>>> matrix = connectivity.generateConnectivityMatrix();

            // list of all algorithm topologies
            List<List<int>> topologies = new List<List<int>>();

            foreach(StaticGraph.IStaticGraph algorithm in staticGraphs)
            {
                topologies.Add(algorithm.topoSort().ToList<int>());
            }

            foreach (DynamicGraph.IDynamicGraph algorithm in dynamicGraphs)
            {
                topologies.Add(algorithm.topology());
            }
            
            
            List<Tuple<int, List<List<bool>>>> resultMatrix = new List<Tuple<int, List<List<bool>>>>();
            // create result matrix, each row is a index + list of comparison results index >= nodeInPath
            // inner most list represents the comparison for each topology
            for (int iFromNode = 0; iFromNode < matrix.Count; iFromNode++)
            {
                resultMatrix.Add(new Tuple<int, List<List<bool>>>(iFromNode, new List<List<bool>>()));

                for (int iToNode = 0; iToNode < matrix[iFromNode].Item2.Count; iToNode++)
                {
                    resultMatrix[iFromNode].Item2.Add(new List<bool>());
                }
            }
            
            // Fill in the resultMatrix
            // TODO this is extremely slow, most likely due to FindIndex. Perhaps use dictionary key = nodeIndex, value = nodeTopology(FindIndex)
            for(int iFromNode = 0; iFromNode < matrix.Count; iFromNode++)
            {
                System.Diagnostics.Debug.WriteLine("Progress comparer: " + (double)iFromNode / (double)matrix.Count * 100 + "%");
                for (int iToNode = 0; iToNode < matrix[iFromNode].Item2.Count; iToNode++)
                {
                    for(int iTopology = 0; iTopology < topologies.Count; iTopology++)
                    {
                        resultMatrix[iFromNode].Item2[iToNode].Add(
                            topologies[iTopology].FindIndex(item => item == matrix[iFromNode].Item1) 
                            >= topologies[iTopology].FindIndex(item => item == matrix[iFromNode].Item2[iToNode]));
                    }
                }
            }

            
            // item => item.Distinct().Skip(1).Any() is applied on the innermost list, Any() returns true if not all algorithms agree
            // Any(item => item == true) is applied twice for the remaining layers, checking if the statement above ever returned a true
            // the result is then negated to answer the question.
            bool allAgree = 
                !resultMatrix.ConvertAll<bool>
                (
                    i => i.Item2.ConvertAll<bool>
                    (
                        item => item.Distinct().Skip(1).Any()
                    ).Any(item => item == true)
                ).Any(item => item == true);

            if (!allAgree)
            {
                writeFile("TopoCompFail");
                throw new Exception("Topological comparison failed");
            }


            return true;
        }

    }
}
