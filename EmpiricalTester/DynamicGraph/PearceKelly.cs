using System;
using System.Collections.Generic;
using System.Linq;

namespace EmpiricalTester.DynamicGraph
{
    public class PearceKelly : IDynamicGraph
    {
        private List<PKNode> nodes;
        private int n;

        public PearceKelly()
        {
            nodes = new List<PKNode>();
            n = 0;
        }

        public void AddVertex()
        {
            nodes.Add(new PKNode(n, n++));
        }

        public bool AddEdge(int ix, int iy)
        {
            var lb = nodes[iy];
            var ub = nodes[ix];

            if (lb.Ord < ub.Ord)
            {
                var F = new List<PKNode>();
                var B = new List<PKNode>();

                var noCycleF = dfsF(lb, ref F, ub.Ord);
                if (!noCycleF)
                {
                    foreach (var v in F)
                    {
                        v.Visited = false;
                    }
                    return false;
                }

                dfsB(ub, ref B, lb.Ord);

                // reorder phase
                F = F.OrderBy(a => a.Ord).ToList();
                B = B.OrderBy(a => a.Ord).ToList();
                
                var L = new List<int>(F.Count + B.Count);

                // The unique identifiers for the topological order of each vertex is added to L
                // L is then sorted and the identifiers (Ord) redistributed over the vertices
                // who are now in the correct order
                for (int i = 0; i < B.Count; i++)
                {
                    L.Add(B[i].Ord);
                    B[i].Visited = false;
                }
                for (int i = 0; i < F.Count; i++)
                {
                    L.Add(F[i].Ord);
                    F[i].Visited = false;
                }
                L.Sort();

                for (int i = 0; i < B.Count; i++)
                {
                    B[i].Ord = L[i];
                }
                for (int i = B.Count; i < L.Count; i++)
                {
                    F[i - B.Count].Ord = L[i];
                }
            }

            ub.Outgoing.Add(lb);
            lb.Incoming.Add(ub);

            return true;
        }

        private bool dfsF(PKNode n, ref List<PKNode> F, int ub)
        {
            n.Visited = true;
            F.Add(n);
            foreach (var w in n.Outgoing)
            {
                if (w.Ord == ub)
                    return false;
                if (!w.Visited && w.Ord < ub)
                {
                    if (dfsF(w, ref F, ub) == false)
                        return false;
                }
            }

            return true;
        }

        private void dfsB(PKNode n, ref List<PKNode> B, int lb)
        {
            n.Visited = true;
            B.Add(n);
            foreach (var w in n.Incoming)
            {
                if(!w.Visited && lb < w.Ord)
                    dfsB(w, ref B, lb);
            }
        }

        public void RemoveEdge(int v, int w)
        {
            throw new NotImplementedException();
        }

        public List<int> Topology()
        {
            return nodes.OrderBy(x => x.Ord).ToList().ConvertAll(a => a.Index);
        }

        public void ResetAll(int newN)
        {
            nodes = new List<PKNode>();
            n = 0;
        }

        public void ResetAll(int newN, int newM)
        {
            ResetAll(newN);
        }
    }
}
