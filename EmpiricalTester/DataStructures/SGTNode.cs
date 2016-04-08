using System;

namespace EmpiricalTester.DataStructures
{
    public class SGTNode<T> : IComparable
    {
        public T Value { get; set; }
        public SGTNode<T> Parent { get; set; }
        public long Label { get; set; }

        public SGTNode<T> Left { get; set; }
        public SGTNode<T> Right { get; set; }

        public SGTNode() { Label = -1; }
        public SGTNode(T value) : this(value, null, null) { }
        public SGTNode(T value, SGTNode<T> left, SGTNode<T> right)
        {
            Value = value;
            Label = -1;
            Left = left;
            Right = right;
        }



        public int CompareTo(object obj)
        {
            return Label.CompareTo(((SGTNode<T>)obj).Label);
        }
    }
}

