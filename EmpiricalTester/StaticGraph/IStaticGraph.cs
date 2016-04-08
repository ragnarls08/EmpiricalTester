
namespace EmpiricalTester.StaticGraph
{
    interface IStaticGraph
    {
        void AddVertex();
        void AddEdge(int v, int w);
        void RemoveEdge(int v, int w);
        int[] TopoSort();
        void ResetAll(); // resets the algorithm to receive a new graph
    }
}
