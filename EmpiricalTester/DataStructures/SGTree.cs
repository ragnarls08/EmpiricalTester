using System;
using System.Collections.Generic;

namespace EmpiricalTester.DataStructures
{
    public class SGTree<T> //where T : IComparable<T>
    {
        public int Count => _n;

        private SGTNode<T> _root;
        private int _n, _q;
        private readonly double _alpha;

        public SGTree(double alpha)
        {
            // q/2 <= n <= q
            _root = null;
            _alpha = alpha;
            _n = 0;
            _q = 0;            
        }


        public virtual void Clear()
        {
            _root = null;
            _n = 0;
            _q = 0;
        }
        
        public bool Query(SGTNode<T> a, SGTNode<T> b)
        {            
            return a.Label > b.Label;
        }

        public bool LT(SGTNode<T> a, SGTNode<T> b)
        {
            return a.Label < b.Label;
        }

        public SGTNode<T> Root
        {
            get
            {
                return _root;
            }
            set
            {
                _root = value;
            }
        }
        
        // only used for debugging
        public int depth()
        {
            return depth(_root) + 1;
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
            _root = root;
            root.Label = long.MaxValue / 2 + 1; 

            _n++;
            _q = _n;

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
                existing.Left.Label = PathLeft(existing.Label);

                newNode = existing.Left;
                newNode.Parent = existing;
            }
            else
            {
                SGTNode<T> curr = existing.Left;

                while (curr.Right != null)
                {
                    curr = curr.Right;
                }

                // found a place for the new element
                curr.Right = newNode;
                curr.Right.Label = PathRight(curr.Label);

                newNode = curr.Right;
                newNode.Parent = curr;
            }

            return InsertHelper(newNode);
        }

        public SGTNode<T> insertAfter(SGTNode<T> existing, T newData)
        {
            return insertAfter(existing, new SGTNode<T>(newData));
        }
            
        public SGTNode<T> insertAfter(SGTNode<T> existing, SGTNode<T> newNode)
        {
            if (existing.Label == -1)
                throw new InvalidOperationException("The node to insert after is not in the tree");
            if (newNode.Label != -1)
                throw new InvalidOperationException("newNode is already in the tree");

            if (existing.Right == null)
            {
                existing.Right = newNode;
                existing.Right.Label = PathRight(existing.Label);

                newNode = existing.Right;
                newNode.Parent = existing;
            }
            else
            {
                SGTNode<T> curr = existing.Right;

                while(curr.Left != null)
                {
                    curr = curr.Left;
                }

                // found a place for the new element
                curr.Left = newNode;
                curr.Left.Label = PathLeft(curr.Label);

                newNode = curr.Left;
                newNode.Parent = curr;
            }
            
            return InsertHelper(newNode);
        }

        private SGTNode<T> InsertHelper(SGTNode<T> newNode)
        {
            _n++;
            _q = Math.Max(_q, _n); // after insertion is set to max(MaxNodeCount, NodeCount)

            // find depth of newNode
            int d = depth(newNode.Label);

            if (d > AlphaLog(_q))
            {
                // depth exceeded, find scapegoat
                SGTNode<T> wChildKnown = newNode;
                SGTNode<T> w = newNode.Parent;

                // start from bottom, size(w) = 1, add 1 every loop and only do size(the other side)
                int sizeW = 2; //newNode + parent (w)
                int sizeLeft = wChildKnown == w.Left ? 1 : Size(w.Left);
                int sizeRight = wChildKnown == w.Right ? 1 : Size(w.Right);


                while (sizeLeft <= _alpha * sizeW &&
                      sizeRight <= _alpha * sizeW)
                {
                    wChildKnown = w;
                    w = w.Parent;

                    sizeLeft = wChildKnown == w.Left ? sizeW : Size(w.Left);
                    sizeRight = wChildKnown == w.Right ? sizeW : Size(w.Right);
                    sizeW = sizeLeft + sizeRight + 1;

                }


                var wParent = w.Parent.Parent; 
                w = w.Parent;
                sizeLeft = wChildKnown == w.Left ? sizeW : Size(w.Left);
                sizeRight = wChildKnown == w.Right ? sizeW : Size(w.Right);
                sizeW = sizeLeft + sizeRight + 1;

                Rebuild(w, sizeW);
                ReLabel(wParent);
            }

            return newNode;
        }

        public void Remove(SGTNode<T> node)
        {
            bool isRoot = node.Parent == null;
            SGTNode<T> relabelNode = null;
            node.Label = -1; // to indiciate this node has been removed, in case: remove(x), insert(x, newData)

            if(node.Left == null && node.Right == null)
            {
                //Console.WriteLine("both null, is root = " + isRoot);
                if (isRoot) 
                    _root = null;
                
                relabelNode = node.Parent;
                RemoveParentsLinkTo(node);
            }
            else if(node.Left == null || node.Right == null)
            {
                //Console.WriteLine("one child, is root = " + isRoot);
                SGTNode<T> child = node.Left ?? node.Right;

                if (!isRoot)
                {
                    ReplaceParentLink(node, child);
                    relabelNode = child.Parent;
                }
                else
                {
                    _root = child;
                    _root.Parent = null;
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
                        _root = rightMost;
                        _root.Right = node.Right;
                        node.Right.Parent = _root;
                        _root.Parent = null;                      
                    }
                    else
                    {                        
                        // move rightMost in nodes place
                        rightMost.Parent = node.Parent;
                        if (node.Parent.Left == node)
                            node.Parent.Left = rightMost;
                        else
                            node.Parent.Right = rightMost;

                        // rightmost was left so it doesnt have any right child
                        // put nodes right child on rightmost right
                        rightMost.Right = node.Right;
                        node.Right.Parent = rightMost;                        
                    }
                }
                else
                {
                    //Console.WriteLine("rm is not left, is root = " + isRoot);
                    //node has 2 children, rightMost is not left
                    if (rightMost.Left != null)
                    {
                        rightMost.Left.Parent = rightMost.Parent;
                        rightMost.Parent.Right = rightMost.Left;
                    }
                    else
                    {
                        rightMost.Parent.Right = null;
                    }

                    if (isRoot)
                    {
                        _root = rightMost;
                        rightMost.Parent = null;
                        rightMost.Left = node.Left;
                        node.Left.Parent = _root;
                        rightMost.Right = node.Right;
                        node.Right.Parent = _root;
                    }
                    else
                    {
                        rightMost.Parent = node.Parent;
                        if (node.Parent.Left == node)
                            node.Parent.Left = rightMost;
                        else
                            node.Parent.Right = rightMost;

                        rightMost.Left = node.Left;
                        node.Left.Parent = rightMost;
                        rightMost.Right = node.Right;
                        node.Right.Parent = rightMost;
                    }                    
                }

                relabelNode = rightMost.Parent;                               
            }

            _n--;

            node.Parent = null;
            node.Right = null;
            node.Left = null;
            
            //NodeCount <= α*MaxNodeCount
            if (_n <= _alpha*_q && _root != null)
            {
                _q = _n;
                Rebuild(_root, _n);
                ReLabel(_root); 
            }
            else if (_root != null)
            {
                ReLabel(isRoot ? _root : relabelNode);
            }                    
        }
        public List<string> inOrderLabels()
        {
            return inOrderLabels(_root, new List<string>());
        }

        /*
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
        */
        private void RemoveParentsLinkTo(SGTNode<T> child)
        {
            if (child == child.Parent?.Left)
                child.Parent.Left = null;
            if (child == child.Parent?.Right)
                child.Parent.Right = null;
        }

        private void ReplaceParentLink(SGTNode<T> oldChild, SGTNode<T> newChild)
        {
            if(oldChild.Parent?.Left == oldChild)
            {
                oldChild.Parent.Left = newChild;
                newChild.Parent = oldChild.Parent;
                oldChild.Parent = null;
            }
            if(oldChild.Parent?.Right == oldChild)
            {
                oldChild.Parent.Right = newChild;
                newChild.Parent = oldChild.Parent;
                oldChild.Parent = null;
            }
        }

        private List<string> inOrderLabels(SGTNode<T> node, List<string> list)
        {
            if (node == null)
                return list;

            inOrderLabels(node.Left, list);
            list.Add(Convert.ToString(node.Label, 2).PadLeft(63, '0'));
            inOrderLabels(node.Right, list);

            return list;
        }
        public List<T> inOrder()
        {
            return inOrder(_root, new List<T>());
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
        private int AlphaLog(int q)
        {
            return (int)Math.Ceiling(Math.Log(q, 1/_alpha)) + 1;
        }
        private int Size(SGTNode<T> node)
        {
            if (node == null)
                return 0;
            else
            {
                int c = 1;
                c += Size(node.Left);
                c += Size(node.Right);

                return c;
            }
        }        
        private int depth(long label)
        {
            long lastBit = label & -label;
            return 62 - (int)Math.Log(lastBit, 2);
        }
        private int PackIntoArray(SGTNode<T> u, SGTNode<T>[] a, int i)
        {
            if (u == null)
            {
                return i;
            }

            i = PackIntoArray(u.Left, a, i);
            a[i++] = u;
            return PackIntoArray(u.Right, a, i);
        }
        private void Rebuild(SGTNode<T> u, int ns)
        {
            SGTNode<T> p = u.Parent;
            SGTNode<T>[] a = new SGTNode<T>[ns];
            PackIntoArray(u, a, 0);

            if (p == null)
            {
                _root = BuildBalanced(a, 0, ns);
                _root.Parent = null;
            }
            else if (p.Right == u)
            {
                p.Right = BuildBalanced(a, 0, ns);
                p.Right.Parent = p; 
            }
            else
            {
                p.Left = BuildBalanced(a, 0, ns);
                p.Left.Parent = p;
            }
        }        
        private SGTNode<T> BuildBalanced(SGTNode<T>[] a, int i, int ns)
        {
            if (ns == 0)
                return null;

            int m = ns / 2;
            a[i+m].Left = BuildBalanced(a, i, m);

            if (a[i + m].Left != null)
                a[i + m].Left.Parent = a[i + m];
            a[i + m].Right = BuildBalanced(a, i + m + 1, ns - m - 1);
            if (a[i + m].Right != null)
                a[i + m].Right.Parent = a[i + m];
            return a[i + m];
        }


        #region Labeling
        // least significant bit moved "right"
        private long PathLeft(long label)
        {
            long lastBit = label & -label;
            return label ^ lastBit | (lastBit >> 1);             
        }
        // least significant bit copied "right"
        private long PathRight(long label)
        {
            long lastBit = label & -label;
            return label | (lastBit >> 1);
        }
        private void ReLabel(SGTNode<T> node)
        {
            if(node == _root)
                node.Label = long.MaxValue / 2 + 1;

            if (node.Left != null)
                RecursiveRelabel(node.Left, PathLeft(node.Label));
            if (node.Right != null)
                RecursiveRelabel(node.Right, PathRight(node.Label));
        }
        private void RecursiveRelabel(SGTNode<T> node, long newLabel)
        {
            node.Label = newLabel;
            if (node.Left != null)
                RecursiveRelabel(node.Left, PathLeft(node.Label));
            if (node.Right != null)
                RecursiveRelabel(node.Right, PathRight(node.Label));
        }
        #endregion Labeling


    }
}
