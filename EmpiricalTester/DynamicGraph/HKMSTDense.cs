using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace EmpiricalTester.DynamicGraph
{
    public class HKMSTDense //: IDynamicGraph
    {
        /*
        private int _n;
        private int _nCounter;
        private HKMSTDNode[] vertex;        
        private bool[,] adjMatrix;

        public HKMSTDense(int n)
        {
            _n = n;
            vertex = new HKMSTDNode[_n];
            adjMatrix = new bool[_n, _n];
        }
        
        public void AddVertex()
        {
            vertex[_nCounter] = new HKMSTDNode(_nCounter);
            _nCounter++;
        }

        public bool AddEdge(int v, int w)
        {
            if (vertex[v].Position > vertex[w].Position)
                if (!TopologicalSearch(vertex[v], vertex[w])) return false;

            adjMatrix[v, w] = true;

            return true;
        }

        private bool TopologicalSearch(HKMSTDNode v, HKMSTDNode w)
        {
            var F = new C5.CircularQueue<HKMSTDNode>();
            var B = new C5.CircularQueue<HKMSTDNode>();

            F.Push(w);
            B.Push(v);

            var i = w.Position;
            var j = v.Position;
            vertex[i] = null;
            vertex[j] = null;
            
            while (true)
            {
                i++;
                while (i < j && !F.Any(u => adjMatrix[u.Position, i])) i++;

                if (i == j)
                    break;

                F.Push(vertex[i]);
                vertex[i] = null;

                j--;
                while(i < j && !B.Any(z => adjMatrix[j, z.Position])) j++;

                if (i == j)
                    break;

                B.Push(vertex[j]);
                vertex[j] = null;
            }

            //Once the search finishes, test for a cycle by checking whether there is an arc(u, z)
            //with u in F and z in B
            bool noCycle = !(from u in F from z in B where adjMatrix[u.Position, z.Position] select u).Any();
            if (!noCycle)
            {
                // cycle was found, insert the nodes back into the order (edge will not be added)
                i = w.Position;
                j = v.Position;

                int initCount = F.Count;
                for (int x = 0; x < initCount; x++)
                    vertex[i++] = F.Dequeue();

                initCount = B.Count;
                for (int x = 0; x < initCount; x++)
                    vertex[j--] = B.Dequeue();
            }
            else
            {
                // Reorder
                while (!F.IsEmpty)
                {
                    if (vertex[i] != null && F.Any(u => adjMatrix[u.Value, vertex[i].Value]))
                    {
                        F.Push(vertex[i]);
                        vertex[i] = null;
                    }

                    if (vertex[i] == null)
                    {
                        var x = F.Dequeue();
                        vertex[i] = x;
                        x.Position = i;
                    }

                    i++;
                }

                while (!B.IsEmpty)
                {
                    j--;
                    if (vertex[j] != null && B.Any(z => adjMatrix[vertex[j].Value, z.Value]))
                    {
                        B.Push(vertex[j]);
                        vertex[j] = null;
                    }

                    if (vertex[j] == null)
                    {
                        var y = B.Dequeue();
                        vertex[j] = y;
                        y.Position = j;
                    }
                }
            }


            return noCycle;

        }//*/

        /*
                private int _n;
                private int _nCounter;
                private int[] vertex;
                private int[] position;
                private bool[,] adjMatrix;


                public HKMSTDense(int n)
                {
                    _n = n;
                    vertex = new int[n];
                    position = new int[n];
                    adjMatrix = new bool[n,n];
                }

                public void AddVertex()
                {
                    vertex[_nCounter] = _nCounter;
                    position[_nCounter] = _nCounter;
                    _nCounter++;
                } 

                public bool AddEdge(int v, int w)
                {
                    if (position[v] > position[w]) 
                        if (!TopologicalSearch(v, w)) return false;
                    
                    adjMatrix[position[v], position[w]] = true;

                    return true;
                }

                private bool TopologicalSearch(int iv, int iw)
                {
                    var F = new C5.CircularQueue<int>();
                    var B = new C5.CircularQueue<int>();

                    F.Push(iw);
                    B.Push(iv);

                    var i = position[iw];
                    var j = position[iv];
                    vertex[i] = -1;
                    vertex[j] = -1;

                    bool notEmpty = true;
                    while (notEmpty)
                    {
                        i++;
                        while (i < j && !F.Any(u => adjMatrix[position[u], i])) i++; // while i<j and no vertex in F has edge to i
                        //while (i < j && !F.Any(u => adjMatrix[u, vertex[i]])) i++; // while i<j and no vertex in F has edge to i

                        if (i == j)
                            break;

                        F.Push(vertex[i]);
                        vertex[i] = -1;


                        j--;
                        while (i < j && !B.Any(z => adjMatrix[j, position[z]])) j--; // while i<j and j does not have an edge to any vertex in B
                        //while (i < j && !B.Any(z => adjMatrix[vertex[j], z])) j--; // while i<j and j does not have an edge to any vertex in B

                        if (i == j)
                            break;

                        B.Push(vertex[j]);
                        vertex[j] = -1;
                    }

                    // cycle check
                    //Once the search finishes, test for a cycle by checking whether there is an arc(u, z)
                    //with u in F and z in B
                    //bool noCycle = !(from u in F from z in B where adjMatrix[position[u], position[z]] select u).Any();
                    bool noCycle = !(from u in F from z in B where adjMatrix[u, z] select u).Any();
                    if (!noCycle)
                    {
                        // cycle was found, insert the nodes back into the order (edge will not be added)
                        i = position[iw];
                        j = position[iv];

                        int initCount = F.Count;
                        for (int x = 0; x < initCount; x++)
                            vertex[i++] = F.Dequeue();

                        initCount = B.Count;
                        for (int x = 0; x < initCount; x++)
                            vertex[j--] = B.Dequeue();
                    }
                    else
                    {
                        // Reorder
                        while (!F.IsEmpty)
                        {
                            if (vertex[i] != -1 && F.Any(u => adjMatrix[u, vertex[i]]))
                            {
                                F.Push(vertex[i]);
                                vertex[i] = -1;
                            }

                            if (vertex[i] == -1)
                            {
                                var x = F.Dequeue();
                                vertex[i] = x;
                                position[x] = i;
                            }

                            i++;
                        }

                        while (!B.IsEmpty)
                        {
                            j--;
                            if (vertex[j] != -1 && B.Any(z => adjMatrix[vertex[j], z]))
                            {
                                B.Push(vertex[j]);
                                vertex[j] = -1;
                            }

                            if (vertex[j] == -1)
                            {
                                var y = B.Dequeue();
                                vertex[j] = y;
                                position[y] = j;
                            }
                        }
                    }


                    return noCycle;
                }

                public void RemoveEdge(int v, int w)
                {
                    //graph[w].Incoming.Remove(v);
                    //graph[w].Outgoing.Remove(v);
                }

                public void ResetAll(int newN)
                {
                    _nCounter = 0;
                    _n = newN;
                    vertex = new int[_n];
                    position = new int[_n];
                    adjMatrix = new bool[_n, _n];
                }

                public List<int> Topology()
                {
                    return vertex.ToList();
                }
                //*/
/*
        private int _n;
        private int _nCounter;
        private int[] _vertex;        
        private bool[,] adjMatrix;


        public HKMSTDense(int n)
        {
            _n = n;
            _vertex = new int[n];
            adjMatrix = new bool[n, n];
        }
        
        public void AddVertex()
        {
            _vertex[_nCounter] = _nCounter;
            _nCounter++;
        }

        public bool AddEdge(int v, int w)
        {
            if (position(v) > position(w))
                if (!TopologicalSearch(v, w)) return false;

            adjMatrix[v, w] = true;

            return true;
        }

        private int vertex(int v)
        {
            for (int i = 0; i < _vertex.Length; i++)
            {
                if (_vertex[i] == v)
                    return i;
            }
            return -1;
        }

        private void vertex(int v, int val)
        {
            for (int i = 0; i < _vertex.Length; i++)
            {
                if (_vertex[i] == v)
                    _vertex[i] = val;
            }
        }

        private int position(int v)
        {
            return _vertex[v];
        }

        private void position(int v, int val)
        {
            _vertex[v] = val;
        }

        private bool TopologicalSearch(int v, int w)
        {
            var F = new C5.CircularQueue<int>();
            var B = new C5.CircularQueue<int>();

            F.Push(w);
            B.Push(v);

            var i = position(w);
            var j = position(v);
            vertex(i, -1);
            vertex(j, -1);
            
            while (true)
            {
                i++;
                while (i < j && !F.Any(u => adjMatrix[position(u), i])) i++; // while i<j and no vertex in F has edge to i

                if (i == j)
                    break;

                F.Push(vertex(i));
                vertex(i, -1);

                j--;
                while (i < j && !B.Any(z => adjMatrix[j, position(z)])) j--; // while i<j and j does not have an edge to any vertex in B
                                                                
                if (i == j)
                    break;

                B.Push(vertex(j));
                vertex(j, -1);
            }

            // cycle check
            //Once the search finishes, test for a cycle by checking whether there is an arc(u, z)
            //with u in F and z in B
            //bool noCycle = !(from u in F from z in B where adjMatrix[position[u], position[z]] select u).Any();
            bool noCycle = !(from u in F from z in B where adjMatrix[u, z] select u).Any();
            if (!noCycle)
            {
                // cycle was found, insert the nodes back into the order (edge will not be added)
                i = vertex[w];
                j = vertex[v];

                int initCount = F.Count;
                for (int x = 0; x < initCount; x++)
                    vertex[i++] = F.Dequeue();

                initCount = B.Count;
                for (int x = 0; x < initCount; x++)
                    vertex[j--] = B.Dequeue();
            }
            else
            {
                // Reorder
                while (!F.IsEmpty)
                {
                    if (vertex(i) != -1 && F.Any(u => adjMatrix[u, vertex(i)]))
                    {
                        F.Push(vertex(i));
                        vertex(i, -1);
                    }

                    if (vertex(i) == -1) 
                    {
                        var x = F.Dequeue();
                        vertex(i, x);
                        position(x, i);
                    }

                    i++;
                }

                while (!B.IsEmpty)
                {
                    j--;
                    if (vertex(j) != -1 && B.Any(z => adjMatrix[vertex(j), z]))
                    {
                        B.Push(vertex(j));
                        vertex(j, -1);
                    }

                    if (vertex(j) == -1)
                    {
                        var y = B.Dequeue();
                        vertex(j, y);
                        position(y, j);
                    }
                }
            }


            return noCycle;
        }

        public void RemoveEdge(int v, int w)
        {
            //graph[w].Incoming.Remove(v);
            //graph[w].Outgoing.Remove(v);
        }

        public void ResetAll(int newN)
        {
            _nCounter = 0;
            _n = newN;
            //vertex = new int[_n];
            
            adjMatrix = new bool[_n, _n];
        }

        public List<int> Topology()
        {
            return null; //vertex.ToList();
        }
        //*/
    }

    internal class HKMSTDNode
    {
        public int Inded { get; set; }
        public int Position { get; set; }

        public HKMSTDNode(int index)
        {
            Inded = index;
            Position = index; //arbitrary in the beginning
        }
    }
}
