using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmpiricalTester.DynamicGraph.Tests
{
    [TestClass()]
    public class SimpleIncrementalTests
    {
        [TestMethod()]
        public void AddEdgeTest()
        {
            var simple = new SimpleIncremental();
            for (int i = 0; i < 5; i++)
                simple.AddVertex();

            var b1 = simple.AddEdge(0, 1);
            var b2 = simple.AddEdge(1, 2);
            var b3 = simple.AddEdge(2, 3);
            var b4 = simple.AddEdge(3, 4);
            var b5 = simple.AddEdge(4, 0);

            var b = b1 && b2 && b3 && b4 && !b5;

            Assert.IsTrue(b);
        }
    }
}