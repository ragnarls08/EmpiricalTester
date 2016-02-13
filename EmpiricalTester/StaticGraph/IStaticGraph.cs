using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpiricalTester.StaticGraph
{
    interface IStaticGraph
    {
        void addVertex();
        void addEdge(int v, int w);
        void removeEdge(int v, int w);
        int[] topoSort();        
    }
}
