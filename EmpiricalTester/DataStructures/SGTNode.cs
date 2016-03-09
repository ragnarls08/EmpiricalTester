using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpiricalTester.DataStructures
{
    class SGTNode<T> : Node<T>, IComparable, IComparable<SGTNode<T>> where T : IComparable<T>
    {
        public SGTNode<T> parent { get; set; }
        public long label { get; set; }

        public SGTNode() : base() { }
        public SGTNode(T data) : base(data, null) { }
        public SGTNode(T data, SGTNode<T> left, SGTNode<T> right)
        {
            base.Value = data;
            NodeList<T> children = new NodeList<T>(2);
            children[0] = left;
            children[1] = right;

            base.Neighbors = children;
        }

        public SGTNode<T> Left
        {
            get
            {
                if (base.Neighbors == null)
                    return null;
                else
                    return (SGTNode<T>)base.Neighbors[0];
            }
            set
            {
                if (base.Neighbors == null)
                    base.Neighbors = new NodeList<T>(2);

                base.Neighbors[0] = value;
            }
        }

        public SGTNode<T> Right
        {
            get
            {
                if (base.Neighbors == null)
                    return null;
                else
                    return (SGTNode<T>)base.Neighbors[1];
            }
            set
            {
                if (base.Neighbors == null)
                    base.Neighbors = new NodeList<T>(2);

                base.Neighbors[1] = value;
            }
        }

        public int CompareTo(object obj)
        {
            if (obj.GetType() != GetType())
                return -1;
            return CompareTo(obj as SGTNode<T>);
        }

        public int CompareTo(SGTNode<T> other)
        {
            if (other == null)
                return -1;
            return this.Value.CompareTo(other.Value);
        }
    }
}

