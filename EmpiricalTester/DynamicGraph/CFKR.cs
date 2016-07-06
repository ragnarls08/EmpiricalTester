using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static EmpiricalTester.DynamicGraph.CFKRNode;

namespace EmpiricalTester.DynamicGraph
{   
    internal enum ReturnMessage { NoCycle, Cycle };

    internal class Message
    {
        public CFKRNode V { get; set; }
        public List<CFKRNode> Label { get; set; }

        public Message(CFKRNode v, List<CFKRNode> label)
        {
            V = v;
            Label = label;
        }
    }

    class CFKR : IDynamicGraph
    {
        const int Infinity = int.MaxValue;

        private List<CFKRNode> nodes;
        private double q;
        private int n;
        private int rankedCount;
        private int visitedCurrentRound;   

        public CFKR(double q)
        {
            this.q = q;
            n = 0;
            rankedCount = 0;
            visitedCurrentRound = 1;
            nodes = new List<CFKRNode>();
        }

        public void AddVertex()
        {
            if(q * (double)n > rankedCount)
                nodes.Add(new CFKRNode(n++, rankedCount++));
            else 
                nodes.Add(new CFKRNode(n++));
        }

        public bool AddEdge(int iu, int iv)
        {
            var u = nodes[iu];
            var v = nodes[iv];

            if (LessThan(u, v))
            {
                u.Outgoing.Add(v);
                v.Incoming.Add(u);
                return true;
            }
                

            visitedCurrentRound++;
            if (CycleDetect(u, v))
                return false;

            
            
            Update(u,v);

            u.Outgoing.Add(v);
            v.Incoming.Add(u);
            return true;
        }

        public void Update(CFKRNode x, CFKRNode y)
        {
            var LCPAndZeta = GetLCP(x, y);

            y.Label.Clear();
            foreach (var l in LCPAndZeta.Item1)
            {
                y.Label.Add(l);
            }
            foreach (var l in LCPAndZeta.Item2)
            {
                y.Label.Add(l);
            }

            if (y.Rank != Infinity)
            {
                y.Label.Add(y);    
            }

            // TODO If `(y) was updated then for all arcs (y, w) ∈ E, recursively apply update(y, w).

            foreach (var node in y.Outgoing)
            {
                Update(y, node);
            }
        }


        // Item1 = LCP
        // Item2 = Zeta
        public Tuple<List<CFKRNode>, List<CFKRNode>> GetLCP(CFKRNode x, CFKRNode y)
        {
            int i = 0;
            while (x.Label.Count > i && y.Label.Count > i && x.Label[i].Index == y.Label[i].Index)
            {
                i++;
            }

            var lcp = new List<CFKRNode>(i+1);
            for (int b = 0; b < i; b++)
            {
                lcp.Add(x.Label[b]);
            }
            var zeta = new List<CFKRNode>();
            for (int b = i; b < x.Label.Count; b++)
            {
                zeta.Add(x.Label[b]);
            }
            var zetaPrime = new List<CFKRNode>();
            if(zeta.Count > 0)
                zetaPrime.Add(zeta[0]);
            for (int b = 1; b < zeta.Count; b++)
            {
                if(zeta[b].Rank < y.Rank)
                    zetaPrime.Add(zeta[b]);
                else
                    break;
            }

            return new Tuple<List<CFKRNode>, List<CFKRNode>>(lcp, zetaPrime);
        } 

        public bool CycleDetect(CFKRNode u, CFKRNode v)
        {
            var backMessage = BVisit(u, new Message(v, u.Label));
            if (backMessage == ReturnMessage.Cycle)
                return true;

            var forwardMessage = FVisit(v, u);
            if (forwardMessage == ReturnMessage.Cycle)
                return true;

            return false;
        }

        public ReturnMessage FVisit(CFKRNode w, CFKRNode u)
        {
            if(w.Visited(visitedCurrentRound) || u.Index == w.Index)
                return ReturnMessage.Cycle;

            if (GreaterThan(u, w) && !w.VisitedForward(visitedCurrentRound))
            {
                foreach (var outNode in w.Outgoing)
                {
                    if(FVisit(outNode, u) == ReturnMessage.Cycle)
                        return ReturnMessage.Cycle;
                }
            }

            return ReturnMessage.NoCycle;
        }

        public ReturnMessage BVisit(CFKRNode w, Message msg)
        {
            if(w.Index == msg.V.Index)
                return ReturnMessage.Cycle;

            if (Equals(w, msg.V) && !w.Visited(visitedCurrentRound))
            {
                foreach (var inNode in w.Incoming)
                {
                    if(BVisit(inNode, msg) == ReturnMessage.Cycle)
                        return ReturnMessage.Cycle;
                }
            }
            return ReturnMessage.NoCycle;
        }

        public void RemoveEdge(int v, int w)
        {
            throw new NotImplementedException();
        }

        public List<int> Topology()
        {
            var bra = new CFKRComparer();
            return nodes.OrderBy(a => a, new CFKRComparer()).ToList().ConvertAll(x => x.Index);
        }

        public void ResetAll(int newN)
        {
            n = 0;
            rankedCount = 0;
            visitedCurrentRound = 1;
            nodes = new List<CFKRNode>();           
        }

        

    }
}
