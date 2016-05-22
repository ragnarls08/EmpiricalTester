using System;
using System.Collections.Generic;
using System.Linq;

namespace EmpiricalTester.DynamicGraph
{
    public class HKMSTDense : IDynamicGraph
    {
        
        private int _n;
        private int _nCounter;
        private HKMSTDNode[] vertex;
        private int[] position;
        private bool[,] adjMatrix; 

        public HKMSTDense(int n)
        {
            _n = n;
            _nCounter = 0;
            vertex = new HKMSTDNode[_n]; 
            position = new int[_n];
            adjMatrix = new bool[_n, _n];
        }
        
        public void AddVertex()
        {
            vertex[_nCounter] = new HKMSTDNode(_nCounter);
            position[_nCounter] = _nCounter;
            _nCounter++;
        }

        public bool AddEdge(int iv, int iw)
        {
            var v = vertex[iv];
            var w = vertex[iw];
            
            if (v.Position > w.Position)
                if (!TopologicalSearch(v, w)) return false;

            adjMatrix[iv, iw] = true;

            return true;
        }

        public void RemoveEdge(int v, int w)
        {
            throw new NotImplementedException();
        }

        public List<int> Topology()
        {
            return position.ToList();
        }

        public void ResetAll(int newN)
        {
            _n = newN;
            _nCounter = 0;
            vertex = new HKMSTDNode[_n];
            position = new int[_n];
            adjMatrix = new bool[_n, _n];
        }

        private bool TopologicalSearch(HKMSTDNode v, HKMSTDNode w)
        {
            var F = new C5.CircularQueue<int>();
            var B = new C5.CircularQueue<int>();

            F.Push(w.Index);
            B.Push(v.Index);

            var i = w.Position;
            var j = v.Position;
            v.Visited = true;
            w.Visited = true;
            
            while (true)
            {
                i++;
                while (i < j && !F.Any(u => adjMatrix[u, vertex[position[i]].Index])) i++;

                if (i == j)
                    break;

                F.Push(vertex[position[i]].Index);
                vertex[position[i]].Visited = true;
                
                j--;
                while(i < j && !B.Any(z => adjMatrix[vertex[position[j]].Index, z])) j--;

                if (i == j)
                    break;

                B.Push(vertex[position[j]].Index);
                vertex[position[j]].Visited = true;
            }

            //Once the search finishes, test for a cycle by checking whether there is an arc(u, z)
            //with u in F and z in B
            bool noCycle = !(from u in F from z in B where adjMatrix[u, z] select u).Any();
            if (!noCycle)
            {
                foreach (var x in F)
                {
                    vertex[x].Visited = false;
                }
                foreach (var x in B)
                {
                    vertex[x].Visited = false;
                }

                return false;
            }
            else
            {
                var fix = new List<HKMSTDNode>();
                // Reorder
                while (!F.IsEmpty)
                {
                    if (vertex[position[i]].Visited == false && F.Any(u => adjMatrix[u, vertex[position[i]].Index]))
                    {
                        F.Push(vertex[position[i]].Index);
                        vertex[position[i]].Visited = true;
                    }

                    if (vertex[position[i]].Visited)
                    {
                        var x = F.Dequeue();
                        position[i] = vertex[x].Index;                        
                        vertex[x].Position = i;
                        fix.Add(vertex[x]);
                    }

                    i++;
                }

                while (!B.IsEmpty)
                {
                    j--;
                    if (vertex[position[j]].Visited == false && B.Any(z => adjMatrix[vertex[position[j]].Index, z]))
                    {
                        B.Push(vertex[position[j]].Index);
                        vertex[position[j]].Visited = true;
                    }

                    if (vertex[position[j]].Visited)
                    {
                        var y = B.Dequeue();
                        position[j] = vertex[y].Index;                        
                        vertex[y].Position = j;
                        fix.Add(vertex[y]);                        
                    }
                }

                foreach (var vert in fix)
                {
                    vert.Visited = false;
                }
            }
            return noCycle;
        }
    }

    internal class HKMSTDNode
    {
        public int Index { get; set; }
        public int Position { get; set; }
        public bool Visited { get; set; }

        public HKMSTDNode(int index)
        {
            Index = index;
            Visited = false;
            Position = index; //arbitrary in the beginning
        }
    }
}
