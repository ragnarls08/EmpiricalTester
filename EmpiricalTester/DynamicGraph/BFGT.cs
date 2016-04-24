using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EmpiricalTester.DynamicGraph 
{
    internal class BFGT : IDynamicGraph
    {
        private List<BFGTNode> _nodes;
        private int _currIndex;
        private int _m;
        private int _delta;
        private int _label;

        private enum ReturnState { Cycle, Normal, Delta };
        

        public BFGT()
        {
            _m = 0;
            _label = 0;
            _currIndex = -1;
            _delta = 1;
            _nodes = new List<BFGTNode>();
        }

        public void ResetAll(int n)
        {
            _m = 0;
            _label = 0;
            _currIndex = -1;
            _delta = 1;
           _nodes = new List<BFGTNode>(n);
        }

        public void RemoveEdge(int a, int b)
        {
            
        }

        public void AddVertex()
        {
            _nodes.Add(new BFGTNode(1, _currIndex--, _label++));
        }

        public bool AddEdge(int iv, int iw)
        {
            var v = _nodes[iv];
            var w = _nodes[iw];
            bool noCycle = true;

            if (!LT(v, w))
                noCycle = Search(v, w);

            if(!noCycle)
                return false;

            // Step 5 insert arc
            v.Outgoing.Add(w);
            if (v.Level == w.Level)
                w.Incoming.Add(v);

            _m++;
            // Let ∆ = min m 1 / 2 ,n 2 / 3
            _delta = Math.Min((int)Math.Pow(_m, 1 / 2.0), (int)Math.Pow(_label, 2 / 3.0));

            //insert arc
            return true;
        }

        private bool Search(BFGTNode v, BFGTNode w)
        {
            var F = new List<BFGTNode>();
            var B = new List<BFGTNode>();
            int arcs = 0;

            var retStateB = BVisit(v, B, ref arcs, w, v);
            var retStateF = ReturnState.Normal;
            //If the search ends without detecting a cycle or traversing at least ∆ arcs, test whether k(w) = k(v).
            if ( retStateB == ReturnState.Normal)
            {
                if (w.Level != v.Level)
                {
                    w.Level = v.Level;
                    w.Incoming.Clear();
                    //search forward
                    retStateF = FVisit(w, F, v, B, w);
                }
            }
            else if (retStateB == ReturnState.Delta)
                retStateF = FVisit(w, F, v, B, w);


            // Step 4 reindex
            if (retStateF != ReturnState.Cycle && retStateB != ReturnState.Cycle)
            {
                B.AddRange(F);
                while (B.Count > 0)
                {
                    var x = B[B.Count - 1];
                    B.RemoveAt(B.Count - 1);
                    x.Visited = false;
                    x.Index = _currIndex--;
                }
            }
            
            return (retStateF != ReturnState.Cycle && retStateB != ReturnState.Cycle);
        }

        private ReturnState BVisit(BFGTNode y, List<BFGTNode> B, ref int arcs, BFGTNode w, BFGTNode v)
        {
            y.Visited = true;
            ReturnState retState = ReturnState.Normal;
            foreach (var inNode in y.Incoming)
            {
                retState = BTraverse(inNode, y, B, ref arcs, w, v);
                if(retState == ReturnState.Cycle)
                    return retState;
                
                if (retState == ReturnState.Delta)
                {
                    y.Visited = false;
                    return retState;
                    //break;
                }
                    
            }

            B.Add(y);

            return retState;
        }

        private ReturnState BTraverse(BFGTNode x, BFGTNode y, List<BFGTNode> B, ref int arcs, BFGTNode w, BFGTNode v)
        {
            if (x == w)
                return ReturnState.Cycle; 

            arcs++;
            if (arcs >= _delta)////////////////////////////////////////////////////
            {
                w.Level = v.Level + 1;
                w.Incoming.Clear();
                B.Clear();
                return ReturnState.Delta; //////////////// unmark and go to step 3
            }
                
            if(!x.Visited)
                return BVisit(x, B, ref arcs, w, v);

            return ReturnState.Normal;
        }

        private ReturnState FVisit(BFGTNode x, List<BFGTNode> F, BFGTNode v, List<BFGTNode> B, BFGTNode w)
        {
            foreach (var outNode in x.Outgoing)
            {
                if (FTraverse(x, outNode, F, v, B, w) == ReturnState.Cycle)
                    return ReturnState.Cycle;
            }
            F.Insert(0, x);

            return ReturnState.Normal;
        }

        private ReturnState FTraverse(BFGTNode x, BFGTNode y, List<BFGTNode> F, BFGTNode v, List<BFGTNode> B, BFGTNode w)
        {
            if (y == v || B.Contains(y))
                return ReturnState.Cycle; ///////////////////////////// cycle

            ReturnState retState;

            if (y.Level <= w.Level)
            {
                y.Level = w.Level;
                y.Incoming.Clear();
                retState = FVisit(y, F, v, B, w);
                if (retState == ReturnState.Cycle)
                    return retState;
            }

            if(y.Level == w.Level)
                y.Incoming.Add(x);

            return ReturnState.Normal;
        }

        private bool LT(BFGTNode v, BFGTNode w)
        {
            // a,b < c,d if and only if either a < b, or a = b and c < d
            // this is wrong??? 
            // wiki lex order: (a,b) ≤ (a′,b′) if and only if a < a′ or (a = a′ and b ≤ b′).            
            if (v.Level < w.Level)
                return true;
            if (v.Level == w.Level && v.Index < w.Index)
                return true;
            
            return false;
        }

        public List<int> Topology()
        {
            var x = _nodes.ConvertAll(i => new Tuple<int, int, int>(i.Label, i.Level, i.Index));
            x.Sort((a, b) =>
            {
                
                if (a.Item2 < b.Item2)
                    return -1;
                if (a.Item2 == b.Item2 && a.Item3 < b.Item3)
                    return -1;
                return 1;
            });

            return x.ConvertAll(i => i.Item1);
        }

        public List<int> Topology2()
        {
            var x = _nodes.ConvertAll(i => new Tuple<int, int, int>(i.Label, i.Level, i.Index));
            x.Sort((a, b) =>
            {

                if (a.Item2 > b.Item2)
                    return -1;
                if (a.Item2 == b.Item2 && a.Item3 > b.Item3)
                    return -1;
                return 1;
            });

            return x.ConvertAll(i => i.Item1);
        }
    }
}
