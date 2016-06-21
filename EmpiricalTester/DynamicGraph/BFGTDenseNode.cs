using System;
using System.Collections.Generic;
using C5;

namespace EmpiricalTester.DynamicGraph
{
    class BFGTDenseNode
    {
        public int Level { get; set; }
        public int InDegree { get; set; }
        public int J { get; set; }
        public int Index { get; set; }
        //with the key of an arc (x, y) equal to the level k(y) of the target vertex
        public IPriorityQueue<KVP> Outgoing { get; set; }
        public Dictionary<int, int> KOut { get; set; }
        public Dictionary<string, int> Bound { get; set; }
        public Dictionary<string, int> Count { get; set; }


        public void AddOutGoing(BFGTDenseNode y)
        {
            Outgoing.Add(new KVP(y.Level, y));
            y.InDegree++;
            Bound.Add($"0-{y.Index}", 1);
            Count.Add($"0-{y.Index}", 0);            
        }
        

        public BFGTDenseNode(int index)
        {
            Level = 1;
            InDegree = 0;
            Index = index;

            //Outgoing = new IntervalHeap<BFGTDenseNode>();
            Outgoing = new IntervalHeap<KVP>();
            KOut = new Dictionary<int, int>();
            Bound = new Dictionary<string, int>();
            Count = new Dictionary<string, int>();
        }

        public BFGTDenseNode(BFGTDenseNode node)
        {
            Level = node.Level;
            InDegree = node.InDegree;
            Index = node.Index;

            KOut = new Dictionary<int, int>();
            foreach (var i in node.KOut)
            {
                KOut.Add(i.Key, i.Value);
            }

            Outgoing = new IntervalHeap<KVP>();
            foreach (var kvp in node.Outgoing)
            {
                Outgoing.Add(new KVP(kvp.Key, kvp.Value));
            }
            Bound = new Dictionary<string, int>();
            foreach (var i in node.Bound)
            {
                Bound.Add(i.Key, i.Value);
            }
            Count = new Dictionary<string, int>();
            foreach (var i in node.Count)
            {
                Count.Add(i.Key, i.Value);
            }
        }

    }
}


// level k(v) and an in-degree d(v) for each vertex v, initially 1 and 0, respectively;
// To add an arc (v, w), if k(v) < k(w), merely add the arc

/*
Algorithm F maintains a level k(v) and an in-degree d(v) for each vertex v, initially
1 and 0, respectively; an approximate level kout(x, y) ≤ k(y) for each arc(x, y); and, for
each vertex v and span j such that 0 ≤ j ≤ max{0, lg d(v)}, a bound b(j, y) storing an
old level of y, and a count c(j, y) recording the number of span-j traversals performed
with the current bound.Initially, b(0, y) = 1 and c(0, y) = 0 for all y.
*/