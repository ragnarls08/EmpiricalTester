using System;
using System.Collections.Generic;

namespace EmpiricalTester.DataStructures
{
    public class SGTree<T> //where T : IComparable<T>
    {
        private SGTNode<T> root;
        private int n, q;
        private double alpha;

        public SGTree(double alpha)
        {
            // q/2 <= n <= q
            root = null;
            this.alpha = alpha;
            n = 0;
            q = 0;            
        }


        public virtual void Clear()
        {
            root = null;
        }
        public int count()
        {
            return n;
        }
        public bool query(SGTNode<T> a, SGTNode<T> b)
        {            
            return a.label > b.label;
        }
        public SGTNode<T> Root
        {
            get
            {
                return root;
            }
            set
            {
                root = value;
            }
        }
        
        // only used for debugging
        public int depth()
        {
            return depth(root) + 1;
        }
        private int depth(SGTNode<T> node)
        {
            if (node == null)
                return 1;

            return Math.Max(depth(node.Left), depth(node.Right)) + 1;
        }

        public SGTNode<T> insertFirst(T newData)
        {
            return insertFirst(new SGTNode<T>(newData));
        }

        public SGTNode<T> insertFirst(SGTNode<T> root)
        {
            this.root = root;
            root.label = (long.MaxValue / 2) + 1; 

            n++;
            q = n;

            return root;
        }

        public SGTNode<T> insertBefore(SGTNode<T> existing, T newData)
        {
            return insertBefore(existing, new SGTNode<T>(newData));
        }

        public SGTNode<T> insertBefore(SGTNode<T> existing, SGTNode<T> newNode)
        {
            if (existing.Left == null)
            {
                existing.Left = newNode;
                existing.Left.label = pathLeft(existing.label);

                newNode = existing.Left;
                newNode.parent = existing;
            }
            else
            {
                SGTNode<T> curr = existing.Left;

                while (curr.Right != null)
                {
                    // TODO special case if bottom is reached. probably wont happens ince tree balances
                    curr = curr.Right;
                }

                // found a place for the new element
                curr.Right = newNode;
                curr.Right.label = pathRight(curr.label);

                newNode = curr.Right;
                newNode.parent = curr;
            }

            return insertHelper(newNode);
        }

        public SGTNode<T> insertAfter(SGTNode<T> existing, T newData)
        {
            return insertAfter(existing, new SGTNode<T>(newData));
        }
            
        public SGTNode<T> insertAfter(SGTNode<T> existing, SGTNode<T> newNode)
        {
            if (existing.label == -1)
                throw new InvalidOperationException("The node to insert after is not in the tree");
            if (newNode.label != -1)
                throw new InvalidOperationException("newNode is already in the tree");

            if (existing.Right == null)
            {
                existing.Right = newNode;
                existing.Right.label = pathRight(existing.label);

                newNode = existing.Right;
                newNode.parent = existing;
            }
            else
            {
                SGTNode<T> curr = existing.Right;

                while(curr.Left != null)
                {
                    // TODO special case if bottom is reached. probably wont happens ince tree balances
                    curr = curr.Left;
                }

                // found a place for the new element
                curr.Left = newNode;
                curr.Left.label = pathLeft(curr.label);

                newNode = curr.Left;
                newNode.parent = curr;
            }
            
            return insertHelper(newNode);
        }

        private SGTNode<T> insertHelper(SGTNode<T> newNode)
        {
            n++;
            q = Math.Max(q, n); // after insertion is set to max(MaxNodeCount, NodeCount)

            // find depth of newNode
            int d = depth(newNode.label);

            if (d > alphaLog(q))
            {
                // depth exceeded, find scapegoat
                SGTNode<T> wChildKnown = newNode;
                SGTNode<T> w = newNode.parent;

                // start from bottom, size(w) = 1, add 1 every loop and only do size(the other side)
                int sizeW = 2; //newNode + parent (w)
                int sizeLeft = wChildKnown == w.Left ? 1 : size(w.Left);
                int sizeRight = wChildKnown == w.Right ? 1 : size(w.Right);


                while (sizeLeft <= alpha * sizeW &&
                      sizeRight <= alpha * sizeW)
                {
                    wChildKnown = w;
                    w = w.parent;

                    sizeLeft = wChildKnown == w.Left ? sizeW : size(w.Left);
                    sizeRight = wChildKnown == w.Right ? sizeW : size(w.Right);
                    sizeW = sizeLeft + sizeRight + 1;

                }


                SGTNode<T> wParent = w.parent.parent; // TODO can this be null? probably
                w = w.parent;
                sizeLeft = wChildKnown == w.Left ? sizeW : size(w.Left);
                sizeRight = wChildKnown == w.Right ? sizeW : size(w.Right);
                sizeW = sizeLeft + sizeRight + 1;

                rebuild(w, sizeW);
                reLabel(wParent);
            }

            return newNode;
        }

        public void remove(SGTNode<T> node)
        {
            bool isRoot = node.parent == null ? true : false;
            SGTNode<T> relabelNode = null;
            node.label = -1; // to indiciate this node has been removed, in case: remove(x), insert(x, newData)

            if(node.Left == null && node.Right == null)
            {
                //Console.WriteLine("both null, is root = " + isRoot);
                if (isRoot) 
                    root = null;
                
                relabelNode = node.parent;
                removeParentsLinkTo(node);
            }
            else if(node.Left == null || node.Right == null)
            {
                //Console.WriteLine("one child, is root = " + isRoot);
                SGTNode<T> child = node.Left != null ? node.Left : node.Right;

                if (!isRoot)
                {
                    replaceParentLink(node, child);
                    relabelNode = child.parent;
                }
                else
                {
                    root = child;
                    root.parent = null;
                }                    
            }
            else
            {
                //Console.WriteLine("two children, is root = " + isRoot);
                SGTNode<T> rightMost = node.Left;

                // find the rightmost node of the left child of node
                while(rightMost.Right != null)
                {
                    rightMost = rightMost.Right;
                }

                //special case
                if(rightMost == node.Left)
                {
                    //Console.WriteLine("rm is left, is root = " + isRoot);
                    if (isRoot)
                    {
                        root = rightMost;
                        root.Right = node.Right;
                        node.Right.parent = root;
                        root.parent = null;                      
                    }
                    else
                    {                        
                        // move rightMost in nodes place
                        rightMost.parent = node.parent;
                        if (node.parent.Left == node)
                            node.parent.Left = rightMost;
                        else
                            node.parent.Right = rightMost;

                        // rightmost was left so it doesnt have any right child
                        // put nodes right child on rightmost right
                        rightMost.Right = node.Right;
                        node.Right.parent = rightMost;                        
                    }
                }
                else
                {
                    //Console.WriteLine("rm is not left, is root = " + isRoot);
                    //node has 2 children, rightMost is not left
                    if (rightMost.Left != null)
                    {
                        rightMost.Left.parent = rightMost.parent;
                        rightMost.parent.Right = rightMost.Left;
                    }
                    else
                    {
                        rightMost.parent.Right = null;
                    }

                    if (isRoot)
                    {
                        root = rightMost;
                        rightMost.parent = null;
                        rightMost.Left = node.Left;
                        node.Left.parent = root;
                        rightMost.Right = node.Right;
                        node.Right.parent = root;
                    }
                    else
                    {
                        rightMost.parent = node.parent;
                        if (node.parent.Left == node)
                            node.parent.Left = rightMost;
                        else
                            node.parent.Right = rightMost;

                        rightMost.Left = node.Left;
                        node.Left.parent = rightMost;
                        rightMost.Right = node.Right;
                        node.Right.parent = rightMost;
                    }                    
                }

                relabelNode = rightMost.parent;                               
            }

            n--;

            node.parent = null;
            node.Right = null;
            node.Left = null;

            if(root != null)
                checkIntegrityDebug();

            //NodeCount <= α*MaxNodeCount
            if (n <= alpha*q && root != null)
            {
                q = n;
                rebuild(root, n);
                reLabel(root); 
            }
            else if (root != null)
            {
                if (isRoot)
                    reLabel(root);
                else
                    reLabel(relabelNode); 
            }                    
        }
        public List<string> inOrderLabels()
        {
            return inOrderLabels(root, new List<string>());
        }

        private void checkIntegrityDebug()
        {
            if (root.parent != null)
                throw new InvalidOperationException("root has parent");
            checkIntegrityDebug(root.Left);
            checkIntegrityDebug(root.Right);
        }

        private void checkIntegrityDebug(SGTNode<T> node)
        {
            if (node == null)
                return;

            if (node.parent.Left != node && node.parent.Right != node)
                throw new InvalidOperationException("linking is wrong");

            checkIntegrityDebug(node.Left);
            checkIntegrityDebug(node.Right);
        }

        private void removeParentsLinkTo(SGTNode<T> child)
        {
            if (child == child.parent?.Left)
                child.parent.Left = null;
            if (child == child.parent?.Right)
                child.parent.Right = null;
        }

        private void replaceParentLink(SGTNode<T> oldChild, SGTNode<T> newChild)
        {
            if(oldChild.parent?.Left == oldChild)
            {
                oldChild.parent.Left = newChild;
                newChild.parent = oldChild.parent;
                oldChild.parent = null;
            }
            if(oldChild.parent?.Right == oldChild)
            {
                oldChild.parent.Right = newChild;
                newChild.parent = oldChild.parent;
                oldChild.parent = null;
            }
        }

        private List<string> inOrderLabels(SGTNode<T> node, List<string> list)
        {
            if (node == null)
                return list;

            inOrderLabels(node.Left, list);
            list.Add(Convert.ToString(node.label, 2).PadLeft(63, '0'));
            inOrderLabels(node.Right, list);

            return list;
        }
        public List<T> inOrder()
        {
            return inOrder(root, new List<T>());
        }
        private List<T> inOrder(SGTNode<T> node, List<T> list)
        {
            if (node == null)
                return list;

            inOrder(node.Left, list);
            list.Add(node.Value);
            inOrder(node.Right, list);

            return list;
        }
        private int alphaLog(int q)
        {
            return (int)Math.Ceiling(Math.Log(q, (1/alpha))) + 1;
        }
        private int size(SGTNode<T> node)
        {
            if (node == null)
                return 0;
            else
            {
                int c = 1;
                c += size(node.Left);
                c += size(node.Right);

                return c;
            }
        }        
        private int depth(long label)
        {
            long lastBit = (label & -label);
            return 62 - (int)Math.Log(lastBit, 2);
        }
        private int packIntoArray(SGTNode<T> u, SGTNode<T>[] a, int i)
        {
            if (u == null)
            {
                return i;
            }

            i = packIntoArray(u.Left, a, i);
            a[i++] = u;
            return packIntoArray(u.Right, a, i);
        }
        private void rebuild(SGTNode<T> u, int ns)
        {
            SGTNode<T> p = u.parent;
            SGTNode<T>[] a = new SGTNode<T>[ns];
            packIntoArray(u, a, 0);

            if (p == null)
            {
                root = buildBalanced(a, 0, ns);
                root.parent = null;
            }
            else if (p.Right == u)
            {
                p.Right = buildBalanced(a, 0, ns);
                p.Right.parent = p; 
            }
            else
            {
                p.Left = buildBalanced(a, 0, ns);
                p.Left.parent = p;
            }
        }        
        private SGTNode<T> buildBalanced(SGTNode<T>[] a, int i, int ns)
        {
            if (ns == 0)
                return null;

            int m = ns / 2;
            a[i+m].Left = buildBalanced(a, i, m);

            if (a[i + m].Left != null)
                a[i + m].Left.parent = a[i + m];
            a[i + m].Right = buildBalanced(a, i + m + 1, ns - m - 1);
            if (a[i + m].Right != null)
                a[i + m].Right.parent = a[i + m];
            return a[i + m];
        }


        #region Labeling
        // least significant bit moved "right"
        private long pathLeft(long label)
        {
            long lastBit = (label & -label);
            return (label ^ lastBit) | (lastBit >> 1);             
        }
        // least significant bit copied "right"
        private long pathRight(long label)
        {
            long lastBit = (label & -label);
            return label | (lastBit >> 1);
        }
        private void reLabel(SGTNode<T> node)
        {
            if(node == root)
                node.label = (long.MaxValue / 2) + 1;

            if (node.Left != null)
                recursiveRelabel(node.Left, pathLeft(node.label));
            if (node.Right != null)
                recursiveRelabel(node.Right, pathRight(node.label));
        }
        private void recursiveRelabel(SGTNode<T> node, long newLabel)
        {
            node.label = newLabel;
            if (node.Left != null)
                recursiveRelabel(node.Left, pathLeft(node.label));
            if (node.Right != null)
                recursiveRelabel(node.Right, pathRight(node.label));
        }
        #endregion Labeling


    }
}
