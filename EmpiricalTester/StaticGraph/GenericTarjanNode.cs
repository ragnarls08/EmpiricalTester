using System.Collections.Generic;

namespace EmpiricalTester.StaticGraph
{
    class GenericTarjanNode<T>
    {
        public List<int> Incoming { get; set; }
        public List<int> Outgoing { get; set; }
        public bool Visited { get; set; }
        public bool TemporaryVisit { get; set; }

        public T Value { get; set; }

        public GenericTarjanNode(T value)
        {
            Incoming = new List<int>();
            Outgoing = new List<int>();
            Visited = false;
            TemporaryVisit = false;
            this.Value = value;
        }
    }
}
