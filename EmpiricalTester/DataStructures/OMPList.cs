using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpiricalTester.DataStructures
{
    class OMPList_OLD
    {
        private double threshold = 1.3; // determines when the tree is rebuilt
        private OMPNode root;
        
        private void createTree(int max)
        {
            // List is only useful here since the nodes are "moved" around instead of the values
            // they will not stay in index "order" from left to right
            // only the node.next/node.previous will keep that order
            List<OMPNode> adjecencyList = new List<OMPNode>();

            root = new OMPNode(0, max);
            root.level = (int)Math.Log(max, 2);
            root.weight = 0;
            root.left = recursiveCreate(0, (int)Math.Floor(((double)max-1.0)/2.0), root, root.level - 1, adjecencyList);            
            root.right = recursiveCreate((int)Math.Ceiling((double)max / 2.0), max, root, root.level - 1, adjecencyList);
            root.isLeaf = false;

            // connect the leafs together
            for (int i = 0; i < adjecencyList.Count-1; i++)
            {
                adjecencyList[i].next = adjecencyList[i + 1];
                adjecencyList[i + 1].previous = adjecencyList[i];
            }
        }

        private OMPNode recursiveCreate(int lower, int upper, OMPNode parent, int level, List<OMPNode> adjecencyList)
        {
            OMPNode self = new OMPNode(lower, upper);

            self.parent = parent;
            self.level = level;

            if (upper - lower <= 0)
            {
                self.isLeaf = true;
                adjecencyList.Add(self);
                return self;
            }

            Tuple<int, int, int, int> split = rangeSplit(lower, upper);

            self.isLeaf = false;            
            self.weight = 0;
            self.left = recursiveCreate(split.Item1, split.Item2, self, level - 1, adjecencyList);
            self.right = recursiveCreate(split.Item3, split.Item4, self, level - 1, adjecencyList);

            return self;
        }

        private Tuple<int, int, int, int> rangeSplit(int lower, int upper)
        {
            int i1, i2, i3, i4 = 0;

            i1 = lower;
            i4 = upper;

            // because of zero base, need floor
            // example lower = 4, upper = 7

            //length = 4 - 7 = 3
            //i2 = 4 + (length/2) = 5 floor
            //i3 = 7 - (length/2) = 6 floor
            
            int length = upper - lower;
            int lengthDiv2 = (int)Math.Floor((double)length / 2.0);
            i2 = lower + lengthDiv2;
            i3 = upper - lengthDiv2;

            System.Diagnostics.Debug.WriteLine(string.Format("({0}, {1}) & ({2}, {3})", i1, i2, i3, i4));

            return new Tuple<int, int, int, int>(i1, i2, i3, i4);
        }

        public OMPList_OLD(double threshold)
        {
            this.threshold = threshold;
            createTree(32);
        }

        public OMPNode InsertFirst(int newNodeValue)
        {
            OMPNode middle = getMiddle(root);
            middle.value = newNodeValue;
            updateStats(middle);
            return middle;
        }

        /// <summary>
        /// Inserts newNode after existingNode
        /// </summary>
        /// <param name="existing"></param>
        /// <param name="newNode"></param>
        public OMPNode Insert(OMPNode existingNode, int newNodeValue)
        {
            // check if the adjecent node is free            
            if(existingNode.next.value != -1)
            {
                //adjecent node is not free, rebalancing required.
                rebalance(existingNode);
            }
            
            if (existingNode.next.value != -1)
            {
                throw new Exception("Rebalance failed");
            }
            
            existingNode.next.value = newNodeValue;
            updateStats(existingNode.next);
            return existingNode.next;                       
        }

        public bool query(OMPNode x, OMPNode y)
        {
            return x.rangeLower > y.rangeLower;
        }

        private void rebalance(OMPNode node)
        {
            // note: the node passed in is a leaf, but not the left most leaf in the 
            //       range that will be selected

            do
            {
                node = node.parent;
            } while (node.isOverflowing(Math.Pow(this.threshold, - node.level)));

            //calculate how many steps we have between 
            int length = node.rangeUpper - node.rangeLower + 1;

            //find left most node
            OMPNode curr = node.left;
            while(!curr.isLeaf)
            {
                curr = curr.left;
            }

            // to connect the redistribution to its neighbours again
            OMPNode leftLink = curr.previous;

            // keeping track of free/inUse nodes for redistribution
            List<OMPNode> free = new List<OMPNode>();
            List<OMPNode> full = new List<OMPNode>();
            List<OMPNode> parents = new List<OMPNode>();

            for (int i = 0; i < length; i++)
            {
                parents.Add(curr.parent);

                if (curr.value == -1)
                    free.Add(curr);
                else
                    full.Add(curr);

                curr = curr.next;
            }

            //each parent will show up twice
            parents = parents.Distinct().ToList();

            // keep track of the right link of the last node
            OMPNode rightLink = curr.next;

            curr = full[0];
            full.RemoveAt(0);// must remove at 0 to keep the order (not reversing it)
            curr.parent = parents[0];

            if (leftLink != null)
            {
                leftLink.next = curr;
                curr.previous = leftLink;
            }                
            else//curr is the leftmost
            {
                curr.previous = null;
            }

            int parentCounter = 1;
            //redistribute the nodes
            while(free.Count > 0 || full.Count > 0)
            {
                if(free.Count > 0)
                {
                    free[0].previous = curr;
                    curr.next = free[0];

                    curr = free[0];
                    free.RemoveAt(0);
                    curr.parent = parents[0];
                    parentCounter++;
                    if (parentCounter % 2 == 0)
                        parents.RemoveAt(0);
                    
                }
                if(full.Count > 0)
                {
                    full[0].previous = curr;
                    curr.next = full[0];

                    curr = full[0];
                    full.RemoveAt(0);
                    curr.parent = parents[0];
                    parentCounter++;
                    if (parentCounter % 2 == 0)
                        parents.RemoveAt(0);

                }
            }

            rightLink.previous = curr;
            curr.next = rightLink;
        }

       

        private void updateStats(OMPNode node)
        {
            do
            {
                node = node.parent;
                node.weight++;
                
            } while (node.parent != null);
        }

        private OMPNode getMiddle(OMPNode root)
        {
            bool left = false;
            OMPNode curr = root.left;

            while (!curr.isLeaf)
            {
                if(left)
                {
                    curr = curr.right;
                    left = true;
                }
                else
                {
                    curr = curr.left;
                    left = false;
                }
            }

            return curr;
        }

       
    }
}
