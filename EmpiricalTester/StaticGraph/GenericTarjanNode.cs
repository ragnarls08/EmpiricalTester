using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpiricalTester.StaticGraph
{
    class GenericTarjanNode<T>
    {
        public List<int> incoming { get; set; }
        public List<int> outgoing { get; set; }
        public bool visited { get; set; }
        public bool temporaryVisit { get; set; }

        public T Value { get; set; }

        public GenericTarjanNode(T value)
        {
            incoming = new List<int>();
            outgoing = new List<int>();
            visited = false;
            temporaryVisit = false;
            this.Value = value;
        }
    }
}
