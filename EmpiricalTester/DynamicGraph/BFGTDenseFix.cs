using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace EmpiricalTester.DynamicGraph
{
    class BFGTDenseFix : IDynamicGraph
    {
        private List<BFGTDenseNode2> nodes;
        private int n;
        private int maxJ;

        private static int[] lookup;

        public bool CycleBackup { get; set; }
        private int test = 0;
        private int test2 = 0;
        
        public BFGTDenseFix()
        {
            n = 0;
            nodes = new List<BFGTDenseNode2>();

            CycleBackup = false; // Set to true for graph generation (when cycles are added) 


            lookup = new int[256];
            for (int i = 1; i < 256; ++i)
            {
                lookup[i] = (int)(Math.Log(i) / Math.Log(2));
            }
        }

        public void AddVertex()
        {
            nodes.Add(new BFGTDenseNode2(n++, maxJ));
        }

        public bool AddEdge(int iv, int iw)
        {
            var v = nodes[iv];
            var w = nodes[iw];

            if (v.Level >= w.Level)
            {
                test++;
                //var A = new List<Edge>();
                var A = new LinkedList<Edge2>();

                // TODO make this more targetted
                var Recovery = CycleBackup ? new List<BFGTDenseNode2>() : null;
                if (CycleBackup)
                {
                    foreach (var node in nodes)
                    {
                        Recovery.Add(new BFGTDenseNode2(node));
                    }
                }

                //Recovery.Add(new BFGTDenseNode(v));
                //Recovery.Add(new BFGTDenseNode(w));

                //A.Add(new Edge(v, w));
                A.AddLast(new Edge2(v, w));

                while (A.Count > 0)
                {
                    var x = A.First.Value.X;
                    var y = A.First.Value.Y;
                    A.RemoveFirst();
                    
                    //var x = A[0].X;
                    //var y = A[0].Y;
                    //A.RemoveAt(0);

                    if (y == v)
                    {
                        if (CycleBackup)
                        {
                            foreach (var node in Recovery)
                            {
                                nodes[node.Index].Level = node.Level;
                                nodes[node.Index].InDegree = node.InDegree;
                                nodes[node.Index].J = node.J;

                                nodes[node.Index].Outgoing = node.Outgoing;
                                nodes[node.Index].KOut = node.KOut;
                                nodes[node.Index].Bound = node.Bound;
                                nodes[node.Index].Count = node.Count;
                            }
                        }

                        return false; // cycle
                    }

                    if (x.Level >= y.Level)
                    {
                        y.Level = x.Level + 1;
                    }
                    else
                    {
                        //int j = (int)Floor(Log(Min(y.Level - x.Level, y.InDegree), 2));
                        int j = LogLookup(Min(y.Level - x.Level, y.InDegree));
                                                
                        x.J = j;

                        //if (x.Count.ContainsKey(j) && x.Count[j].ContainsKey(y.Index))
                        if (x.Count[j].ContainsKey(y.Index))
                            x.Count[j][y.Index]++;
                        else
                        {
                            //if (!x.Count.ContainsKey(j))
                            //x.Count.Add(j, new Dictionary<int, int>());
                            x.Count[j].Add(y.Index, 1);
                        }

                        //if (x.Count[j][y.Index] == (3 * Pow(2, j)))
                        if (x.Count[j][y.Index] == (3 * IntPow(2, (uint)j)))
                        {
                            x.Count[j][y.Index] = 0;

                            //int bound = x.Bound.ContainsKey(j) && x.Bound[j].ContainsKey(y.Index) ? x.Bound[j][y.Index] : 1;
                            int bound = x.Bound[j].ContainsKey(y.Index) ? x.Bound[j][y.Index] : 1;
                            //y.Level = Max(y.Level, bound + (int)Pow(2, j));
                            y.Level = Max(y.Level, bound + IntPow(2, (uint)j));
                            //if (!x.Bound.ContainsKey(j))
                                //x.Bound.Add(j, new Dictionary<int, int>());
                            if (!x.Bound[j].ContainsKey(y.Index))
                                x.Bound[j].Add(y.Index, y.Level);
                            else
                                x.Bound[j][y.Index] = y.Level;
                        }
                    }

                    if (!y.Outgoing.IsEmpty)
                    {
                        var curr = y.Outgoing.FindMin();
                        while (curr.Key <= y.Level)
                        {
                            y.Outgoing.DeleteMin();
                            A.AddLast(new Edge2(y, curr.Value));
                            //Recovery.Add(new BFGTDenseNode(curr.Value));
                            if (y.Outgoing.IsEmpty)
                                break;
                            curr = y.Outgoing.FindMin();
                            
                            test2++;
                        }
                    }
                    x.Outgoing.Add(new KVP2(y.Level, y));
                }
            }
            
            v.AddOutGoing(w);

            return true;
        }

        public void RemoveEdge(int v, int w)
        {
            throw new NotImplementedException();
        }

        public List<int> Topology()
        {
            return nodes.OrderBy(n => n.Level).ToList().ConvertAll(i => i.Index);
        }

        public void ResetAll(int newN)
        {
            nodes.Clear();
            n = 0;
            maxJ = LogLookup(newN)+1;

            Console.WriteLine("Dense: " + test);
            Console.WriteLine("Dense_: " + test2);
            test = 0;
            test2 = 0;
        }

        public void ResetAll(int newN, int newM)
        {
            ResetAll(newN);
            Console.WriteLine("Dense: " + test);
            test = 0;
        }

        private int IntPow(int x, uint pow)
        {
            int ret = 1;
            while (pow != 0)
            {
                if ((pow & 1) == 1)
                    ret *= x;
                x *= x;
                pow >>= 1;
            }
            return ret;
        }

        private static int LogLookup(int i)
        {
            if (i >= 0x1000000) { return lookup[i >> 24] + 24; }
            else if (i >= 0x10000) { return lookup[i >> 16] + 16; }
            else if (i >= 0x100) { return lookup[i >> 8] + 8; }
            else { return lookup[i]; }
        }
    }

    internal class Edge2
    {
        public BFGTDenseNode2 X { get; set; }
        public BFGTDenseNode2 Y { get; set; }

        public Edge2(BFGTDenseNode2 x, BFGTDenseNode2 y)
        {
            X = x;
            Y = y;
        }
    }

    internal class KVP2 : IComparable
    {
        public int Key { get; set; }
        public BFGTDenseNode2 Value { get; set; }

        public KVP2(int key, BFGTDenseNode2 value)
        {
            Key = key;
            Value = value;
        }

        public int CompareTo(object obj)
        {
            return Key.CompareTo(((KVP2)obj).Key);
        }
    }




}
