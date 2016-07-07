using System;
using System.Collections.Generic;
using System.Linq;

namespace EmpiricalTester.DynamicGraph 
{
    internal class BFGT : IDynamicGraph
    {
        private enum ReturnState { Cycle, Normal, Delta };

        private List<BFGTNode> nodes;
        private int a; // current index
        private int b;
        private int n;
        private int m;
        private int delta;
        private int totalM;

        public bool CycleBackup { get; set; }
        
        public BFGT()
        {
            a = int.MaxValue / 10 - 1;                        
            n = 0;
            m = 0;
            totalM = 25; // For runtime tests, ResetAll is called prior to every graph giving totalM
            delta = Math.Min((int)Math.Pow(totalM, 1 / 2.0), (int)Math.Pow(n, 2 / 3.0));
            nodes = new List<BFGTNode>();

            CycleBackup = false; // Set to true for graph generation (when cycles are added) 
        }
        
        public void AddVertex()
        {
            nodes.Add(new BFGTNode(0, a--, n++, b));  
        }

        public bool AddEdge(int iv, int iw)
        {
            var v = nodes[iv];
            var w = nodes[iw];

            var F = new List<BFGTNode>();
            var B = new List<BFGTNode>();
            var L = new List<BFGTNode>();

            if ((v.BK + v.Index) > (w.BK + w.Index))
            {
                //step 2                
                var arcs = 0;
                var wIncomingTemp = CycleBackup ? new List<int>(w.Incoming) : null;
                var wTempLevel = w.Level;

                var retStateB = BVisit(v, B, ref arcs, w);
                if (retStateB == ReturnState.Cycle)
                {
                    foreach (var node in B)
                    {
                        node.Visited = false;
                    }
                    return false;
                }
                
                if (retStateB == ReturnState.Normal && v.Level == w.Level)
                {
                    L = B;
                }
                else
                {
                    bool do3 = false;
                    if (retStateB == ReturnState.Normal && w.Level < v.Level)
                    {
                        w.Level = v.Level;
                        w.Incoming.Clear();
                        do3 = true;
                    }
                    else if (retStateB == ReturnState.Delta)
                    {
                        w.Level = v.Level + 1;
                        B.Clear();
                        B.Add(v);
                        w.Incoming.Clear();
                        do3 = true;
                    }
                    if (do3)
                    {
                        // step 3
                        var backupStack = CycleBackup ? new List<BFGTNode>() : null;
                        var retStateF = FVisit(w, F, v, B, backupStack);

                        if (retStateF == ReturnState.Cycle)
                        {
                            if (CycleBackup)
                            {
                                w.Level = wTempLevel;
                                w.Incoming = wIncomingTemp;

                                foreach (var node in backupStack)
                                {
                                    nodes[node.Label] = node;
                                    nodes[node.Label].Visited = false;
                                }
                            }

                            return false;
                        }

                        if (v.Level < w.Level)
                            L = F;
                        if (v.Level == w.Level)
                        {
                            L = B;
                            L.AddRange(F);
                        }
                    }
                }

                //step 4
                while (L.Count > 0)
                {
                    var x = L[L.Count - 1];
                    L.RemoveAt(L.Count - 1);
                    x.Visited = false;
                    x.Index = a--;                                       
                }
            }
            
            //step 5
            v.Outgoing.Add(w.Label);
            if(v.Level == w.Level)
                w.Incoming.Add(v.Label);

            m++;
            //delta = Math.Min((int)Math.Pow(m, 1 / 2.0), (int)Math.Pow(n, 2 / 3.0));
            
            return true;
        }

        public void RemoveEdge(int v, int w)
        {
            throw new NotImplementedException();
        }

        public List<int> Topology()
        {
            return nodes.OrderBy(n => n.BK + n.Index).ToList().ConvertAll(i => i.Label);
        }

        public void ResetAll(int newN)
        {
            a = (int)Math.Pow(newN, 3) + 1;
            b = (int)Math.Pow(newN, 3) + newN + 1;
            m = 0;
            n = 0;
            totalM = 6;
            delta = Math.Min((int)Math.Pow(totalM, 1 / 2.0), (int)Math.Pow(newN, 2 / 3.0));
            nodes.Clear();            
        }

        public void ResetAll(int newN, int newM)
        {
            a = (int)Math.Pow(newN, 3) + 1;
            b = (int)Math.Pow(newN, 3) + newN + 1;
            m = 0;
            n = 0;
            totalM = newM;
            delta = Math.Min((int)Math.Pow(totalM, 1 / 2.0), (int)Math.Pow(newN, 2 / 3.0));
            nodes.Clear();
        }

        private ReturnState BVisit(BFGTNode y, List<BFGTNode> B, ref int arcs, BFGTNode w)
        {
            y.Visited = true;
            var retState = ReturnState.Normal;
            foreach (var inNode in y.Incoming)
            {
                retState = BTraverse(nodes[inNode], y, B, ref arcs, w);
                if (retState == ReturnState.Cycle)
                {
                    y.Visited = false;
                    return retState;
                }
                    
                if (retState == ReturnState.Delta)
                {
                    y.Visited = false;
                    break;
                }
            }
            B.Add(y);
            y.Visited = false;
            return retState;
        }

        private ReturnState BTraverse(BFGTNode x, BFGTNode y, List<BFGTNode> B, ref int arcs, BFGTNode w)
        {
            if (x == w)
                return ReturnState.Cycle;

            arcs++;
            if (arcs >= delta)
            {
                return ReturnState.Delta; 
            }

            if (!x.Visited)
                return BVisit(x, B, ref arcs, w);

            return ReturnState.Normal;
        }

        private ReturnState FVisit(BFGTNode x, List<BFGTNode> F, BFGTNode v, List<BFGTNode> B, List<BFGTNode> backupStack)
        {
            x.Visited = true;
            foreach (var outNode in x.Outgoing)
            {
                if (CycleBackup && !nodes[outNode].Visited)
                {
                    backupStack.Add(new BFGTNode(nodes[outNode].Level, nodes[outNode].Index, nodes[outNode].Label, b, new List<int>(nodes[outNode].Incoming), new List<int>(nodes[outNode].Outgoing)));
                }
                    
                if (FTraverse(x, nodes[outNode], F, v, B, backupStack) == ReturnState.Cycle)
                {
                    x.Visited = false;
                    return ReturnState.Cycle;
                }
                    
            }
            F.Insert(0, x);

            return ReturnState.Normal;
        }

        private ReturnState FTraverse(BFGTNode x, BFGTNode y, List<BFGTNode> F, BFGTNode v, List<BFGTNode> B, List<BFGTNode> backupStack)
        {
            if (y == v || B.Contains(y))
                return ReturnState.Cycle; 

            if (x.Level == y.Level)
                y.Incoming.Add(x.Label);

            if (x.Level > y.Level) // Todo should x be w
            {
                y.Level = x.Level;
                y.Incoming.Clear();
                y.Incoming.Add(x.Label);

                if (!y.Visited)
                    return FVisit(y, F, v, B, backupStack);
            }
            
            return ReturnState.Normal;
        }
        
    }
}
