using System;
using System.Collections.Generic;

namespace EmpiricalTester.DynamicGraph
{
    class CFKRNode //: IComparable
    {
        public int Index { get; set; } // name        
        public int Rank { get; set; }
        public List<CFKRNode> Label { get; set; }
        public List<CFKRNode> Incoming { get; set; }
        public List<CFKRNode> Outgoing { get; set; }

        private int visitedRound;
        private int visitedRoundForward;

        public CFKRNode(int index)
        {
            Index = index;
            Rank = int.MaxValue;
            Incoming = new List<CFKRNode>();
            Outgoing = new List<CFKRNode>();
            Label = new List<CFKRNode>();
            //Label.Add(this);
        }

        public CFKRNode(int index, int rank)
        {
            Index = index;
            Rank = rank;
            Incoming = new List<CFKRNode>();
            Outgoing = new List<CFKRNode>();
            Label = new List<CFKRNode>();
            Label.Add(this);
        }

        public bool Visited(int visitedRound)
        {
            if (this.visitedRound == visitedRound)
                return true;
            else
            {
                this.visitedRound = visitedRound;
                return false;
            }
        }

        public bool VisitedForward(int visitedRound)
        {
            if (this.visitedRoundForward == visitedRound)
                return true;
            else
            {
                this.visitedRoundForward = visitedRound;
                return false;
            }
        }


        public static bool GreaterThan(CFKRNode lhs, CFKRNode rhs)
        {
            if (lhs.Label.Count > rhs.Label.Count)
                return true;
            if (lhs.Label.Count < rhs.Label.Count)
                return false;

            for (int i = 0; i < lhs.Label.Count; i++)
            {
                if (lhs.Label[i].Rank < rhs.Label[i].Rank /*|| (lhs.Label[i].Rank == Infinity && rhs.Label[i].Rank == Infinity)*/)
                    return false;
            }

            return true;
        }

        public static bool LessThan(CFKRNode lhs, CFKRNode rhs)
        {
            if (lhs.Label.Count < rhs.Label.Count)
                return true;
            if (lhs.Label.Count > rhs.Label.Count)
                return false;

            for (int i = 0; i < lhs.Label.Count; i++)
            {
                if (i != lhs.Label.Count - 1)
                {
                    if (lhs.Label[i].Rank > rhs.Label[i].Rank /*|| (lhs.Label[i].Rank == Infinity && rhs.Label[i].Rank == Infinity)*/)
                        return false;
                }                    
                else if (lhs.Label[i].Rank >= rhs.Label[i].Rank)
                    return false;


            }

            return true;
        }

        public static bool Equals(CFKRNode lhs, CFKRNode rhs)
        {
            if (lhs.Label.Count != rhs.Label.Count)
                return false;
            for (int i = 0; i < lhs.Label.Count; i++)
            {
                if (lhs.Label[i].Rank != rhs.Label[i].Rank)
                    return false;
            }
            return true;
        }
        /*
        public int CompareTo(object obj)
        {
            if (LessThan(this, (CFKRNode) obj))
                return -1;
            if (GreaterThan(this, (CFKRNode) obj))
                return 1;
            return 0;
        }*/
    }

    internal class CFKRComparer : IComparer<CFKRNode>
    {
        public int Compare(CFKRNode x, CFKRNode y)
        {
            //return x.Index.CompareTo(y.Index);
                

            
            if (CFKRNode.LessThan(x, y))
                return -1;
            if (CFKRNode.GreaterThan(x, y))
                return 1;
            return 0;
        }
    }
}
