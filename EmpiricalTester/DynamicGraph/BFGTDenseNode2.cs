using System.Collections.Generic;
using C5;

namespace EmpiricalTester.DynamicGraph
{
    class BFGTDenseNode2
    {
        public int Level { get; set; }
        public int InDegree { get; set; }
        public int J { get; set; }
        public int Index { get; set; }
        //with the key of an arc (x, y) equal to the level k(y) of the target vertex
        public IPriorityQueue<KVP2> Outgoing { get; set; }
        public Dictionary<int, int> KOut { get; set; }
         
        //public Dictionary<int, Dictionary<int, int>> Bound { get; set; }
        //public Dictionary<int, Dictionary<int, int>> Count { get; set; }

        public Dictionary<int, int>[] Bound { get; set; }
        public Dictionary<int, int>[] Count { get; set; }



        public void AddOutGoing(BFGTDenseNode2 y)
        {
            Outgoing.Add(new KVP2(y.Level, y));
            y.InDegree++;

            if(!Bound[0].ContainsKey(y.Index))
                Bound[0].Add(y.Index, 1);
            if(!Count[0].ContainsKey(y.Index))
                Count[0].Add(y.Index, 0);

            /*
            if (!Bound.ContainsKey(0))
            {
                Bound.Add(0, new Dictionary<int, int>());
                Bound[0].Add(y.Index, 1);
            }
            if (!Count.ContainsKey(0))
            {
                Count.Add(0, new Dictionary<int, int>());
                Count[0].Add(y.Index, 0);
            }*/

        }


        public BFGTDenseNode2(int index, int maxJ)
        {
            Level = 1;
            InDegree = 0;
            Index = index;

            Outgoing = new IntervalHeap<KVP2>();
            KOut = new Dictionary<int, int>();
            //Bound = new Dictionary<int, Dictionary<int, int>>();
            //Count = new Dictionary<int, Dictionary<int, int>>();

            Bound = new Dictionary<int, int>[maxJ];
            Count = new Dictionary<int, int>[maxJ];

            for (int i = 0; i < maxJ; i++)
            {
                Bound[i] = new Dictionary<int, int>();
                Count[i] = new Dictionary<int, int>();
            }
        }

        public BFGTDenseNode2(BFGTDenseNode2 node)
        {
            Level = node.Level;
            InDegree = node.InDegree;
            Index = node.Index;

            KOut = new Dictionary<int, int>();
            foreach (var i in node.KOut)
            {
                KOut.Add(i.Key, i.Value);
            }

            Outgoing = new IntervalHeap<KVP2>();
            foreach (var kvp in node.Outgoing)
            {
                Outgoing.Add(new KVP2(kvp.Key, kvp.Value));
            }

            
            /*
            Bound = new Dictionary<int, Dictionary<int, int>>();
            foreach (var i in node.Bound)
            {
                Bound.Add(i.Key, new Dictionary<int, int>());
                foreach (var i1 in i.Value)
                {
                    Bound[i.Key].Add(i1.Key, i1.Value);
                }
            }
            Count = new Dictionary<int, Dictionary<int, int>>();
            foreach (var i in node.Count)
            {
                Count.Add(i.Key, new Dictionary<int, int>());
                foreach (var i1 in i.Value)
                {
                    Count[i.Key].Add(i1.Key, i1.Value);
                }
            }*/
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
