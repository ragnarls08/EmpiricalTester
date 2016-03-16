
namespace EmpiricalTester.DataStructures
{
    public class SGTNode<T> 
    {
        public T Value { get; set; }
        public SGTNode<T> parent { get; set; }
        public long label { get; set; }

        public SGTNode<T> Left { get; set; }
        public SGTNode<T> Right { get; set; }

        public SGTNode() { this.label = -1; }
        public SGTNode(T value) : this(value, null, null) { }
        public SGTNode(T value, SGTNode<T> left, SGTNode<T> right)
        {
            this.Value = value;
            this.label = -1;
            this.Left = left;
            this.Right = right;
        }              
    }
}

