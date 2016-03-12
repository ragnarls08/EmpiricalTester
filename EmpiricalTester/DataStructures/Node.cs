using System;

namespace EmpiricalTester.DataStructures
{
    public class Node<T>
    {
        // Private member-variables
        private T data;
        public virtual Node<T> Left { get; set; }
        public virtual Node<T> Right { get; set; }

        public Node() { }
        public Node(T data) : this(data, null, null) { }
        public Node(T data, Node<T> left, Node<T> right)
        {
            this.data = data;
            this.Right = right;
            this.Left = left;
        }

        public T Value
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

       
    }
}