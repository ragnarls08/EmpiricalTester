using System.Collections.Generic;

namespace EmpiricalTester.DynamicGraph
{
    internal interface IDynamicGraph
    {
        void AddVertex();
        bool AddEdge(int v, int w);
        void RemoveEdge(int v, int w);
        List<int> Topology();
        void ResetAll(int newN); // resets the algorithm to receive a new graph
        void ResetAll(int newN, int newM); // resets the algorithm to receive a new graph
    }
}
