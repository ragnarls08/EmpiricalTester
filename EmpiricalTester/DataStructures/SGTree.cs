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

        public SGTree()
        {
            // q/2 <= n <= q
            root = null;
            n = 0;
            q = 0;
        }

        public bool Add(T data)
        {
            SGTNode<T> newNode = new SGTNode<T>(data);
            int d = addWithDepth(newNode);

            if (d > Log32(q))
            {
                // depth exceeded, find scapegoat
                SGTNode<T> w = newNode.parent;
                while (3 * size(w) <= 2 * size(w.parent))
                {
                    w = w.parent;
                }

                rebuild(w.parent);
            }

            return d >= 0;
        }

        public void InOrder()
        {
            InOrder(root);
        }

        private static int Log32(int q)
        {
            double log32 = 2.4663034623764317;
            return (int)Math.Ceiling(log32 * Math.Log(q));
        }

        private void InOrder(SGTNode<T> node)
        {
            if(node != null)
            {
                InOrder(node.Left);
                Console.WriteLine(node.Value);
                InOrder(node.Right);
            }
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

        int packIntoArray(SGTNode<T> u, SGTNode<T>[] a, int i)
        {
            if(u == null)
            {
                return i;
            }

            i = packIntoArray(u.Left, a, i);
            a[i++] = u;
            return packIntoArray(u.Right, a, i);
        }

        SGTNode<T> buildBalanced(SGTNode<T>[] a, int i, int ns)
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


        private int addWithDepth(SGTNode<T> node)
        {
            SGTNode<T> w = root;

            if(w == null)
            {
                root = node;
                n++;
                q++;

                return 0;
            }

            bool done = false;
            int d = 0;

            do
            {
                if(node.CompareTo(w) < 0)
                {
                    if(w.Left == null)
                    {
                        w.Left = node;
                        node.parent = w;
                        done = true;
                    }
                    else
                    {
                        w = w.Left;
                    }
                }
                else if(node.Value.CompareTo(w.Value) > 0)
                {
                    if(w.Right == null)
                    {
                        w.Right = node;
                        node.parent = w;
                        done = true;
                    }
                    w = w.Right;
                }
                else
                {
                    return -1;
                }

                d++;
            } while (!done);

            n++;
            q++;

            return d;
        }

        public virtual void Clear()
        {
            root = null;
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
    }
}
