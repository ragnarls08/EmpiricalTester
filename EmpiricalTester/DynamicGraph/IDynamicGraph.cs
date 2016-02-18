using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpiricalTester.DynamicGraph
{
    interface IDynamicGraph
    {
        void addVertex();
        bool addEdge(int v, int w);
        void removeEdge(int v, int w);        
    }
}
