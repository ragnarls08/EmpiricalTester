using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpiricalTester.DataStructures
{
    public class SGTNode<T> : Node<T>//, IComparable, IComparable<SGTNode<T>> where T : IComparable<T>
    {
        public SGTNode<T> parent { get; set; }
        public long label { get; set; }

        public SGTNode() : base() { }
        public SGTNode(T data) : base(data, null, null) { }
        public SGTNode(T data, SGTNode<T> left, SGTNode<T> right) : base(data, left, right) { }
        
        public new SGTNode<T> Left
        {
            get { return (SGTNode<T>)base.Left; }
            set { base.Left = value; }
        }

        public new SGTNode<T> Right
        {
            get { return (SGTNode<T>)base.Right; }
            set { base.Right = value; }
        }
        /*
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
        }*/
    }
}

