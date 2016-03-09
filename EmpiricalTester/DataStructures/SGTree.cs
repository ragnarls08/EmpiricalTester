using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpiricalTester.DataStructures
{
    class SGTree<T> where T : IComparable<T>
    {
        private SGTNode<T> root;
        private int n, q;
        private double alpha = 0.5;

        public SGTree()
        {
            // q/2 <= n <= q
            root = null;
            n = 0;
            q = 0;            
        }


        public virtual void Clear()
        {
            root = null;
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
        public SGTNode<T> insertFirst(T newData)
        {
            root = new SGTNode<T>(newData);
            root.label = (long.MaxValue / 2) + 1;

            n++;
            q++;

            return root;
        }
        public SGTNode<T> insert(SGTNode<T> existing, T newData)
        {
            SGTNode<T> newNode;

            if(existing.Right == null)
            {
                existing.Right = new SGTNode<T>(newData);
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
                curr.Left = new SGTNode<T>(newData);
                curr.Left.label = pathLeft(curr.label);

                newNode = curr.Left;
                newNode.parent = curr;
            }

            n++;
            q++;

            // find depth of newNode
            int d = depth(newNode.label);
                        
            if(d > alphaLog(q))
            {
                // depth exceeded, find scapegoat
                SGTNode<T> w = newNode.parent;

                // TODO fix the size, optimize
                while(size(w.Left) <= alpha * size(w) &&
                      size(w.Right) <= alpha * size(w))
                {
                    w = w.parent;
                }
                SGTNode<T> wParent = w.parent.parent; // TODO can this be null? probably
                // note: will root.left always be null?

                rebuild(w.parent);
                reLabel(wParent);
            }

            return newNode;
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
        private void rebuild(SGTNode<T> u)
        {
            int ns = size(u);
            SGTNode<T> p = u.parent;
            SGTNode<T>[] a = new SGTNode<T>[ns];
            packIntoArray(u, a, 0);

            if (p == null)
            {
                SGTNode<T> r = buildBalanced(a, 0, ns);
                r.parent = null;
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
