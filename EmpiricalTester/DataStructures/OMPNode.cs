using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpiricalTester.DataStructures
{
    public class OMPNode
    {
        public int rangeLower { get; set; }
        public int rangeUpper { get; set; }

        public OMPNode left { get; set; }
        public OMPNode right { get; set; }
        public OMPNode parent { get; set; }

        //only used for leafs
        public OMPNode previous { get; set; }
        public OMPNode next { get; set; }
                
        public bool isLeaf { get; set; }
        public int level { get; set; }
        public int weight { get; set; } // how many are occupied below
        public int value { get; set; }
        

        public OMPNode(int lower, int upper)
        {
            this.rangeLower = lower;
            this.rangeUpper = upper;
            this.value = -1; // not in use
        }

        public double density()
        {
            return (double)weight / (double)(rangeUpper - rangeLower + 1);
        }

        public bool isOverflowing(double t)
        {
            return density() > t;
        }
    }
}
