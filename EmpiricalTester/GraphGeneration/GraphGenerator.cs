using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpiricalTester.GraphGeneration
{
    class GraphGenerator
    {
        public string generateGraph(int n, double p)
        {
            DateTime datenow = DateTime.Now;
            string filename = datenow.ToString("yyyyMMdd-HH:MM") + string.Format("({0}, {1}).txt", n.ToString(), p.ToString());

            List<Tuple<int, int>> edges = new List<Tuple<int, int>>();

            StaticGraph.IStaticGraph kahn = new StaticGraph.Kahn();
            StaticGraph.IStaticGraph tarjan = new StaticGraph.Tarjan();

            Random random = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < n; i++)
            {
                kahn.addVertex();
                tarjan.addVertex();
            }

            for(int i = 0; i < n; i++)
            {
                Console.WriteLine("Progress " + (double)i/(double)n*100 +"%");
                if(random.Next(0,100)/100.0 < p)
                {
                    int v = random.Next(0, n - 1);
                    int w = random.Next(0, n - 1);

                    kahn.addEdge(v, w);
                    tarjan.addEdge(v, w);

                    int[] k = kahn.topoSort();
                    int[] t = tarjan.topoSort();

                    if((k != null && t == null) || (k == null && t != null))
                    {
                        throw new Exception("Tarjan and Kahn to not agree");
                    }
                    
                    // cycle, remove this edge
                    if(k == null && t == null)
                    {
                        kahn.removeEdge(v, w);
                        tarjan.removeEdge(v, w);
                    }
                    else
                    {
                        edges.Add(new Tuple<int, int>(v, w));
                    }
                }
            }

            bool compare = sortCompare(tarjan.topoSort(), kahn.topoSort(), edges);

            if(!compare)
            {
                throw new Exception("Topological order comparison failed");
            }

            return filename;
        }


        // Compares the edges topological order
        private bool sortCompare(int[] a, int[] b, List<Tuple<int, int>> edges)
        {
            if (a.Length != b.Length)
                return false;

            List<int> aList = a.ToList<int>();
            List<int> bList = b.ToList<int>();
            
            foreach(Tuple<int, int> edge in edges)
            {
                bool aCompare = aList.FindIndex(item => item == edge.Item1) >= aList.FindIndex(item => item == edge.Item2);
                bool bCompare = bList.FindIndex(item => item == edge.Item1) >= bList.FindIndex(item => item == edge.Item2);

                if (aCompare != bCompare)
                    return false;
            }
            
            return true;
        }

    }
}
